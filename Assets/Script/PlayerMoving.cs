using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class PlayerMoving : DamageController
{
    // Start is called before the first frame update
    [HideInInspector]public Rigidbody2D rigid;
    SpriteRenderer SR;
    Animator anim;
    AudioSource audio_Source;
    CapsuleCollider2D CC;
    public GameManger gameManger;
    public PlayerPortal playerPortal;
    public float Speed; // 이동 속도
    public float MaxSpeed; // 최고 속도
    public float JumpPower; // 점프력
    public float DoubleJumpPower; // 더블 점프력
    public float MaxFallSpeed; // 추락 속도
    public float FallDamage; // 낙뎀
    public float PlayerAtkDmg; // 플레이어 공격력
    [HideInInspector]public static float TotalPlayerDamage;   // 플레이어 최종공격력
    [HideInInspector]public static float Damagefix; // 공격력 수정
    public float PlayerHp; // 플레이어 체력
    public int PlayerLevel; // 플레이어 레벨
    float h; //Horizontal 방향
    bool bottomjump; // 하단점프
    int jumpcnt = 0; // 점프 카운트
    int playerlayer, PDL, floorlayer; // 점프 시 바닥 뚫기 레이어
    public GameObject[] Circle;
    public Slider hpSlider;  //hp 슬라이더UI
    public Slider expSlider; //exp 슬라이더UI
    public Slider EnergySlider; // attack Energy 슬라이더UI
    //public Slider CircleSlider; // circle skill 슬라이더 //1215 from jeongik
    public GameObject hudDamageText; //데미지 텍스트
    public GameObject healingText; //힐링 텍스트
    public GameObject LevelUpText; // 레벨업 텍스트
    public Transform hudPos; //데미지 텍스트 위치
    [HideInInspector] public float CurrentExp; //현재 exp
    public float[] NeedExp;  //레벨업할때 필요한 경험치
    [HideInInspector] public float CurrentHp;
    [HideInInspector] public static float CurrentEnergy;
    public float CircleEnergy;
    public static float EnergySpeed;
    float EnemyDamage;  //적 데미지
    [HideInInspector] public static float SkillDamage;
    GameObject scanObject;
    public int Money;
    public int AbilityPoint;
    PlayerStatText playerStatText;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CC = GetComponent<CapsuleCollider2D>();
        playerStatText = GetComponent<PlayerStatText>();
        playerlayer = LayerMask.NameToLayer("Player");
        PDL = LayerMask.NameToLayer("PlayerDamaged"); // 플레이어 피격시 레이어
        floorlayer = LayerMask.NameToLayer("normalfloor");
        anim.SetBool("jumping", true);
        audio_Source = GetComponent<AudioSource>();
        hp = PlayerHp;
        CurrentHp = PlayerHp;
        CurrentExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 서클 선택
        if (Input.GetKeyDown(KeyCode.Z))//1207 from jeongik
        {
            Circle[0].SetActive(true);
        }    

        // 플레이어 데미지 1~1.5배 사이 지정 //1214from jeongik
        TotalPlayerDamage = PlayerAtkDmg * Random.Range(1.0f, 1.5f);//1214from jeongik
        SkillDamage = PlayerAtkDmg * 5;
        //Debug.Log(Input.GetKey(KeyCode.DownArrow));

        // 하단 점프
        //Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
        //RaycastHit2D rayHit3 = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("floor"));
        //Debug.Log(rayHit3.collider.name);
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetButtonDown("Jump"))
        {
            Physics2D.IgnoreLayerCollision(playerlayer, floorlayer, true);
            Physics2D.IgnoreLayerCollision(PDL, floorlayer, true);

            bottomjump = true;
            
        }


        //바닥 감지
        if ((bottomjump && !anim.GetBool("jumping")) || (rigid.velocity.y< -0.0001f && !anim.GetBool("jumping")))
        {
            Debug.DrawRay(rigid.position+(Vector2.down*0.5f), Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position+(Vector2.down*0.5f), Vector2.down, 1, LayerMask.GetMask("normalfloor"));
            if (rayHit.collider == null)
            {
                anim.SetBool("jumping", true);
                jumpcnt += 1;
                //Debug.Log(jumpcnt);

                bottomjump = false;
            }
        }

        //점프
        if (!Input.GetKey(KeyCode.DownArrow) && Input.GetButtonDown("Jump") && jumpcnt < 2)
        {
            if (jumpcnt == 1){
                rigid.velocity = Vector2.up * DoubleJumpPower;
            }
            else if (jumpcnt == 0){
                rigid.velocity = Vector2.up * JumpPower;
            }
            Physics2D.IgnoreLayerCollision(playerlayer, floorlayer, true);
            Physics2D.IgnoreLayerCollision(PDL, floorlayer, true);
                                            // 점프 시 충돌 무시
            anim.SetBool("jumping", true);
            jumpcnt += 1;
            //Debug.Log(jumpcnt);
        }

        // 스탑

        if (Input.GetButtonUp("Horizontal") || h == 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.01f, rigid.velocity.y);

        }


        //좌우 캐릭터 방향
        if (Input.GetButton("Horizontal"))
        {
            if (h == 1)
            {
                SR.flipX = false;
            }
            else if (h == -1)
            {
                SR.flipX = true;
            }
        }

        //이동 애니메이션
        if ((h == 0))
        {
            anim.SetBool("running", false);
        }
        else
        {
            anim.SetBool("running", true);
        }

        //이동 애니메이션 해제
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("running", false);
        }
        else
        {
            anim.SetBool("running", true);
        }

        //랜딩
        if (anim.GetBool("jumping"))
        {
            Debug.DrawRay(rigid.position + Vector2.down * 0.49f+(Vector2.down*0.49f), Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position + Vector2.down * 0.49f+(Vector2.down*0.49f), Vector2.down, 1, LayerMask.GetMask("normalfloor", "blockedfloor"));
            Debug.DrawRay(rigid.position + new Vector2(0.3f, -0.49f)+(Vector2.down*0.49f), Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit3 = Physics2D.Raycast(rigid.position + new Vector2(0.3f, -0.49f)+(Vector2.down*0.48f), Vector2.down, 1, LayerMask.GetMask("normalfloor", "blockedfloor"));
            Debug.DrawRay(rigid.position + new Vector2(-0.3f, -0.49f)+(Vector2.down*0.49f), Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit4 = Physics2D.Raycast(rigid.position + new Vector2(-0.3f, -0.49f)+(Vector2.down*0.48f), Vector2.down, 1, LayerMask.GetMask("normalfloor", "blockedfloor"));
            Debug.DrawRay(rigid.position+(Vector2.down*0.6f), Vector2.up, new Color(1, 0, 0));
            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position+(Vector2.down*0.6f), Vector2.up, 1, LayerMask.GetMask("normalfloor", "blockedfloor"));
            //Debug.Log(rayHit2.collider);
            if (rigid.velocity.y <= 0 && rayHit2.collider == null)
            {
                //Debug.Log(rayHit.collider.name);
                //Debug.Log(anim.GetBool("jumping"));

                Physics2D.IgnoreLayerCollision(playerlayer, floorlayer, false);
                Physics2D.IgnoreLayerCollision(PDL, floorlayer, false);
                if (rayHit.collider != null || rayHit3.collider != null || rayHit4.collider != null)
                {
                    //Debug.Log(rigid.velocity.y);
                    if (rigid.velocity.y < -25){
                        //Debug.Log(rigid.velocity.y);
                        //Debug.Log(FallDamage*Mathf.Log10(Mathf.Abs(rigid.velocity.y)));
                        gameObject.GetComponent<DamageController>().PlayerOnDamage(FallDamage*Mathf.Abs(rigid.velocity.y));
                    }
                    anim.SetBool("jumping", false);
                    jumpcnt = 0;
                    //Debug.Log(jumpcnt);
                    
                }
            }
        }


        //hp, exp 슬라이더관련 조정
        hpSlider.maxValue = PlayerHp;
        //hpSlider.value = CurrentHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, CurrentHp, Time.deltaTime * 10); //선형보간 함수로 체력이 부드럽게 깎임
        expSlider.maxValue = NeedExp[PlayerLevel - 1];
        expSlider.value = CurrentExp;
        // CircleSlider.maxValue = 50;
        // if (CircleEnergy >50 ){
        //     CircleEnergy = 50;
        //     CircleSlider.value = CircleEnergy;
        // }
        // else{
        //     CircleSlider.value = CircleEnergy;
        // }
        EnergySlider.maxValue = 100f;
        EnergySlider.value = CurrentEnergy; // FixedUpdate에서 Energy 채우는 스크립트 한 줄

        // 레벨업시
        if (CurrentExp >= NeedExp[PlayerLevel - 1])
        {
            CurrentExp = 0;
            PlayerLevel++;
            PlayerHp += 5; //1215 from jeongik
            CurrentHp = PlayerHp;
            hp = PlayerHp;
            AbilityPoint += 3;
            PlayerAtkDmg += 2;
            //AngleSpeed += 1;
            //Size += 0.1f;

            playerStatText.StatTextUpLoad();

            GameObject hudText = Instantiate(LevelUpText); //Levelup 텍스트 생성
            hudText.transform.position = hudPos.position;
            hudText.transform.Translate(Vector3.left * 4, Space.World); //LEVEL UP 텍스트 표시 위치 좌측으로 고정이동

        }
        if (CurrentHp != hp && CurrentHp > hp)   //체력 감소시
        {
            float dmg = CurrentHp - hp;
            GameObject hudText = Instantiate(hudDamageText);
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = (int)dmg;

            gameManger.HealthDown(dmg);
            gameObject.layer = 12;
            //SR.color = new Color(1, 1, 1, 0.4f);
            TurnColor();
            //int Direction = transform.position.x - Target_Position.x > 0 ? 1 : -1;
            //rigid.AddForce(new Vector2(Direction, 1) * 7, ForceMode2D.Impulse);

            //anim.SetTrigger("Damaged");
            Invoke("OffDamaged", 2);
        }
        if (CurrentHp > PlayerHp)   // 체력회복시
        {
            CurrentHp = PlayerHp;
            hp = CurrentHp;
        }

        if (exp > CurrentExp)   // exp gain
        {
            CurrentExp = exp;
        }

        if (Input.GetKeyDown(KeyCode.T) && (scanObject != null))     // T를 누를경우 상호작용           
        {
            gameManger.SearchAction(scanObject);
        }
        if (Input.GetKeyDown(KeyCode.T) && (scanObject == null))     // T를 누를경우 상호작용           
        {
            if (gameManger.isTalkPanelActive == true)
            {
                gameManger.talkIndex = 0;
                gameManger.isTalkPanelActive = false;
                gameManger.talkPanel.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.F1))    // F1를 누를경우 도움말           
        {
            gameManger.HelpButton();
        }
        if (Input.GetKeyDown(KeyCode.F2))    // F2를 누를경우 플레이어스텟창           
        {
            gameManger.PlayerStatButton();
        }
        if (Input.GetKeyDown(KeyCode.F3))    // F3 make portal           
        {     
            
              playerPortal.PortalButton();  // need Fix 1217                     
        }
    }

    void FixedUpdate()
    {

        //좌우 이동
        h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h * Speed, ForceMode2D.Impulse);

        //속도 제한
        if (rigid.velocity.x > MaxSpeed)
        {
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < MaxSpeed * (-1))
        {
            rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);
        }

        //오브젝트 스캔
        Debug.DrawRay(rigid.position, Vector3.left * 1f, new Color(0, 1, 0));
        Debug.DrawRay(rigid.position, Vector3.right * 1f, new Color(0, 1, 0));
        RaycastHit2D rayLeftHit = Physics2D.Raycast(rigid.position, Vector3.left, 1f,LayerMask.GetMask("SearchObject"));
        RaycastHit2D rayRightHit = Physics2D.Raycast(rigid.position, Vector3.right, 1f, LayerMask.GetMask("SearchObject"));
        if (rayLeftHit.collider != null)
        {
            scanObject = rayLeftHit.collider.gameObject;
        }
        else if(rayRightHit.collider != null)
        {
            scanObject = rayRightHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }


    }
   
// 피격 판정
void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag ("Enemy"))
        {
            EnemyDamage = collision.gameObject.GetComponent<DamageController>().EnemyAtkDmg;
            gameObject.GetComponent<DamageController>().PlayerOnDamage(EnemyDamage);
            int Direction = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(Direction, 1) * 7, ForceMode2D.Impulse);
            //OnDamaged(collision.transform.position, EnemyDamage);
        }
        else if (collision.gameObject.CompareTag("Money"))
        {
            MoneyData moneyData = collision.gameObject.GetComponent<MoneyData>();
            Money += moneyData.moneyAmount;
            playerStatText.PlayerMoney_Text();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Potion_Heal"))
        {
            float Heal = PlayerHp * 0.2f;
            GameObject hudText = Instantiate(healingText);
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = (int)Heal;
            collision.gameObject.SetActive(false);
            CurrentHp += PlayerHp * 0.2f;
            hp = CurrentHp;
            if (CurrentHp > PlayerHp)
            {
                CurrentHp = PlayerHp;
                hp = CurrentHp;
            }
        }
        //if (collision.gameObject.tag == "EnemyFly")
        //{
        //    Debug.Log("hit");
        //    if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        //    {
        //        OnAttackFly(collision.transform);
        //    }
        //    else
        //    {
        //        EnemyDamage = collision.gameObject.GetComponent<EnemyFlyMove>().EnemyAtk;
        //        OnDamaged(collision.transform.position);
        //    }
        //}
        //if (collision.gameObject.tag == "EnemyBoss")
        //{
        //    Debug.Log("hit");
        //    if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        //    {
        //        OnAttackBoss(collision.transform);
        //    }
        //    else
        //    {
        //        EnemyDamage = collision.gameObject.GetComponent<EnemyBossMove>().EnemyAtk;
        //        OnDamaged(collision.transform.position);
        //    }
        //}
        //if (collision.gameObject.tag == "EnemyBossTest")
        //{
        //    Debug.Log("hit");
        //    if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        //    {
        //        OnAttackBossTest(collision.transform);
        //    }
        //    else
        //    {
        //        EnemyDamage = collision.gameObject.GetComponent<EnemyBossMoveTest>().EnemyAtk;
        //        OnDamaged(collision.transform.position);
        //    }
        //}
        //if (collision.gameObject.tag == "EnemyRangedAttack")
        //{
        //    Debug.Log("hit");
        //    if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        //    {
        //        OnAttackEnemyRangedAttack(collision.transform);
        //    }
        //    else
        //    {
        //        EnemyDamage = collision.gameObject.GetComponent<EnemyRangedAttack>().EnemyAtk;
        //        OnDamaged(collision.transform.position);
        //    }
        //}
        //if (collision.gameObject.tag == "EnemyBomb")
        //{

        //    EnemyDamage = collision.gameObject.GetComponent<EnemyBomb>().EnemyAtk;
        //    OnDamaged(collision.transform.position);

        //}

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "JumpBox" && Input.GetButton("Jump"))
        {
            rigid.AddForce(Vector2.up * 50, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterMap")
        {
            rigid.gravityScale = 1.5f;
            //rigid.velocity = new Vector2(rigid.velocity.x * 0.5f,rigid.velocity.y);
            rigid.drag = 0.25f;
            if (MaxSpeed > 10)
            {
                MaxSpeed = 10;
            }
        }
        if (collision.gameObject.CompareTag("LeftExcelZone"))
        {
            rigid.AddForce(Vector2.left * 100, ForceMode2D.Force);
        }
        if (collision.gameObject.CompareTag("UpExcelZone"))
        {
            rigid.AddForce(Vector2.up * 20, ForceMode2D.Force);
        }
        
    }

    //public void OnDamaged(Vector2 Target_Position, float damage)
    //{
    //    GameObject hudText = Instantiate(hudDamageText);
    //    hudText.transform.position = hudPos.position;
    //    hudText.GetComponent<DamageText>().damage = (int)damage;

    //    gameManger.HealthDown(damage);
    //    gameObject.layer = 12;
    //    //SR.color = new Color(1, 1, 1, 0.4f);
    //    TurnColor();
    //    int Direction = transform.position.x - Target_Position.x > 0 ? 1 : -1;
    //    rigid.AddForce(new Vector2(Direction, 1) * 7, ForceMode2D.Impulse);

    //    //anim.SetTrigger("Damaged");
    //    Invoke("OffDamaged", 2);

    //}

    void OffDamaged()
    {
        gameObject.layer = 3;
        SR.color = new Color(1, 1, 1, 1);
    }

    void TurnColor()
    {
        SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
        Invoke("ReTurnColor", 0.5f);
    }
    void ReTurnColor()
    {
        SR.color = new Color(1, 1, 1, 0.5f);
        Invoke("TurnColor1", 0.5f);
    }

    void TurnColor1()
    {
        SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
        Invoke("ReTurnColor1", 0.5f);
    }
    void ReTurnColor1()
    {
        SR.color = new Color(1, 1, 1, 0.5f);
    }

    //지상몹 공격
    //public void OnAttack(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    //    EnemyMove enemymove = Enemy.GetComponent<EnemyMove>();
    //    enemymove.EnemyDamaged(TotalPlayerDamage);

    //}
    //공중몹 공격
    //public void OnAttackFly(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

    //    EnemyFlyMove enemyFlyMove = Enemy.GetComponent<EnemyFlyMove>();
    //    enemyFlyMove.EnemyDamaged(TotalPlayerDamage);
    //}
    //보스몹 공격
    //public void OnAttackBoss(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

    //    EnemyBossMove enemyBossMove = Enemy.GetComponent<EnemyBossMove>();
    //    enemyBossMove.EnemyDamaged(TotalPlayerDamage);
    //}
    //public void OnAttackBossTest(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    //    EnemyBossMoveTest enemyBossMoveTest = Enemy.GetComponent<EnemyBossMoveTest>();
    //    enemyBossMoveTest.EnemyDamaged(TotalPlayerDamage);
    //}
    //public void OnAttackEnemyRangedAttack(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    //    EnemyRangedAttack enemyRangedAttack = Enemy.GetComponent<EnemyRangedAttack>();
    //    enemyRangedAttack.EnemyDamaged(TotalPlayerDamage);
    //}
    //public void OnAttackEnemyBomb(Transform Enemy)

    //{
    //    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    //    EnemyBomb enemyBomb = Enemy.GetComponent<EnemyBomb>();
    //    enemyBomb.EnemyDamaged(TotalPlayerDamage);
    //}




    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Item_1")
        {
            collision.gameObject.SetActive(false);
            // Circle[1].SetActive(true);
            // Circle[2].SetActive(false);
            // Circle[3].SetActive(false);

        }
        else if (collision.gameObject.tag == "Item_2")
        {
            collision.gameObject.SetActive(false);
            // Circle[1].SetActive(false);
            // Circle[2].SetActive(true);
            // Circle[3].SetActive(false);

        }
        else if (collision.gameObject.tag == "Item_3")
        {
            collision.gameObject.SetActive(false);
            // Circle[1].SetActive(false);
            // Circle[2].SetActive(false);
            // Circle[3].SetActive(true);

        }
        else if (collision.gameObject.tag == "Potion_Heal")
        {
            float Heal = PlayerHp * 0.2f;
            GameObject hudText = Instantiate(healingText);
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = (int)Heal;
            collision.gameObject.SetActive(false);
            CurrentHp += PlayerHp * 0.2f;
            hp = CurrentHp;
            if (CurrentHp > PlayerHp)
            {
                CurrentHp = PlayerHp;
                hp = CurrentHp;
            }

        }
        else if (collision.gameObject.tag == "Finish")
        {
            collision.gameObject.SetActive(false);
            gameManger.SceneSave();
            gameManger.NextStage();
        }
        else if (collision.gameObject.tag == "ItemSlowSkill")
        {
            gameManger.ItemSlowSkill();
            collision.gameObject.SetActive(false);
            //Destroy(collision);
        }
        
        //else if(collision.gameObject.tag=="Finish")
        //{
        //    collision.gameObject.SetActive(false);
        //    gameManger.Stage_Score += 1000;
        //    gameManger.NextStage();
        //}
        //else if (collision.gameObject.tag == "Bullet")
        //{
        //    Debug.Log("hit");

        //    EnemyDamage = collision.gameObject.GetComponent<EnemyBullet>().bulletDamage;
        //    OnDamaged(collision.transform.position);
        //}
        //else if (collision.gameObject.tag == "TraceBullet")
        //{
        //    Debug.Log("hit");

        //    EnemyDamage = collision.gameObject.GetComponent<EnemyTraceBullet>().bulletDamage;
        //    OnDamaged(collision.transform.position);
        //}      
    }
    public void OnDie()
    {
        SR.color = new Color(1, 0, 1, 0.5f);
        SR.flipY = true;
        CC.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        rigid.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    // public void PlaySound(string action)
    // {
    //     switch (action)
    //     {
    //         case "JUMP":
    //             audio_Source.clip = Audio_Jump;
    //             break;
    //         case "ATTACK":
    //             audio_Source.clip = Audio_Attack;
    //             break;
    //         case "DAMAGED":
    //             audio_Source.clip = Audio_Damaged;
    //             break;
    //         case "ITEM":
    //             audio_Source.clip = Audio_Item;
    //             break;
    //         case "DIE":
    //             audio_Source.clip = Audio_Die;
    //             break;
    //         case "FINISH":
    //             audio_Source.clip = Audio_Finish;
    //             break;
    //     }
    //     audio_Source.Play();

    // }

}
