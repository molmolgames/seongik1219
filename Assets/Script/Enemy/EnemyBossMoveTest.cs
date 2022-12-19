using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossMoveTest : EnemyController
{
    // Rigidbody2D En;
    // Animator animation;
    // SpriteRenderer SR;
    // CapsuleCollider2D CC;
    Vector3 enemyDirectionVector;
    // public PlayerMoving player;
    public int Next_Move_X;
    public int Next_Move_Y;
    // public float speed; // �̵� �ӵ�
    // public float EnemyHP;
    // public float EnemyCurrentHp;
    // public float EnemyAtk;
    // public float EnemyExp; // �� ����ġ
    // public GameObject hudDamageText; //������ �ؽ�Ʈ
    // public Transform hudPos; //������ �ؽ�Ʈ ��ġ
    // public Slider enemyHpSlider;
    public Transform box; // �÷��̾� ���� �ڽ�
    public Vector2 boxSize; // // �÷��̾� ���� �ݰ�
    // public GameObject dropItem;
    // int var = 0;  //���� �׾�� �����̴� �� ������

    public GameObject bullet;
    public Transform bulletPosition;
    public float coolTime;
    private float currentTime;
    void Awake()
    {
        En = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        CC = GetComponent<CapsuleCollider2D>();
        EnemyCurrentHp = EnemyHP;
        enemyHpSlider.gameObject.SetActive(false);
        Think();

        Invoke("Think", 3);
        hp = EnemyCurrentHp;
        EnemyAtkDmg = EnemyAtk;

    }

    void Update()
    {
        if (En.velocity.x < 0)
        {
            SR.flipX = false;
        }
        else if (En.velocity.x > 0)
        {
            SR.flipX = true;
        }
        ////raycast direction set
        //if (En.velocity.y > 0)
        //{
        //    enemyDirectionVector = Vector3.up;
        //}
        //else if (En.velocity.y < 0)
        //{
        //    enemyDirectionVector = Vector3.down;
        //}
        //else if (En.velocity.x < 0)
        //{
        //    enemyDirectionVector = Vector3.left;
        //}
        //else if (En.velocity.x > 0)
        //{
        //    enemyDirectionVector = Vector3.right;
        //}

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;

        // �÷��̾� ����
        //Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(box.position, boxSize, 0);
        //foreach (Collider2D CC in collider2Ds)
        //{
        //    if (CC.tag == "Player")
        //    {
        //        Next_Move_X = 0;
        //        Next_Move_Y = 0;
        //        transform.position = Vector3.MoveTowards(transform.position, CC.transform.position, Time.deltaTime * speed); 
        //    }

        //}
        if (var == 0)
        {
            StartCoroutine("Trace");
        }

        else if (var == 1)
        {
            StopCoroutine("Trace");
        }

        currentTime -= Time.deltaTime;
        if (EnemyCurrentHp != hp && EnemyCurrentHp > 0)
        {
            float dmg = EnemyCurrentHp - hp;
            GameObject hudText = Instantiate(hudDamageText);
            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = (int)dmg;
            EnemyCurrentHp = hp;

            ActiveHpSlider();

            if (EnemyCurrentHp <= 0)
            {
                SR.color = new Color(1, 0, 1, 0.5f);
                SR.flipY = true;
                CC.enabled = false;
                var = 1;
                Invoke("DeActive", 1);
                Invoke("UnActiveHpSlider", 1);
                Invoke("DropItem", 1f);
                StopMove();
                exp += EnemyExp;
                //Invoke("Revive", 10);
            }
            else
            {
                Invoke("UnActiveHpSlider", 5);
                En.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
                Invoke("ReturnEnemyColor", 1);
            }
        }
    }


    void FixedUpdate()
    {
        //move
        En.velocity = new Vector2(Next_Move_X, Next_Move_Y);

        //Platform check
        //Vector2 Front_Vec = new Vector2(En.position.x + Next_Move * 0.5f, En.position.y);
        //Debug.DrawRay(En.position, Vector3.down, new Color(0, 1, 0));

        //RaycastHit2D rayHit = Physics2D.Raycast(En.position, enemyDirectionVector, 1f,
        //   LayerMask.GetMask("Wall"));
        //Debug.DrawRay(En.position, enemyDirectionVector, new Color(0, 1, 0));

        //if (rayHit.CC != null)
        //{
        //    EnemyTurn();
        //}
        //Debug.DrawRay(En.position + Vector2.right * -10, Vector2.left, new Color(0, 1, 0);
        //RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -10, 10, isLayer);
        //if (box.collision.gameObject.tag == "Player")
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, raycast.CC.transform.position, Time.deltaTime * speed);
        //}
        //RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -10, 10, isLayer);

    }

    void Think()
    {
        Next_Move_X = Random.Range(-3, 4);
        Next_Move_Y = Random.Range(-2, 3);
        float Next_Think_Time = Random.Range(2f, 4f);
        Invoke("Think", Next_Think_Time);
    }
    //void EnemyTurn()
    //{
    //    Next_Move_X = Next_Move_X * -1;
    //    SR.flipX = Next_Move_X == 1;
    //    Invoke("Think", 3);

    //}

    //public void EnemyDamaged(float damage)
    //{
    //    GameObject hudText = Instantiate(hudDamageText);
    //    hudText.transform.position = hudPos.position;
    //    hudText.GetComponent<DamageText>().damage = (int)damage;
    //    EnemyCurrentHp = EnemyCurrentHp - damage;
    //    ActiveHpSlider();
    //    if (EnemyCurrentHp <= 0)
    //    {
    //        SR.color = new Color(1, 0, 1, 0.5f);
    //        SR.flipY = true;
    //        CC.enabled = false;
    //        var = 1;
    //        Invoke("UnActiveHpSlider", 1);
    //        Invoke("DeActive", 1);
    //        Invoke("DropItem", 1.2f);
    //        StopMove();

    //        player.CurrentExp += EnemyExp;
    //        Invoke("Revive", 10);
    //    }
    //    else
    //    {
    //        Invoke("UnActiveHpSlider", 5);
    //        En.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
    //        SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
    //        Invoke("ReturnEnemyColor", 1);
    //    }
    //}

    // public void DeActive()
    // {
    //     gameObject.SetActive(false);
    // }
    // public void ReturnEnemyColor()
    // {
    //     SR.color = new Color(1, 1, 1, 1);
    // }
    // public void Revive()
    // {
    //     hp = EnemyHP;
    //     EnemyCurrentHp = EnemyHP;
    //     gameObject.SetActive(true);
    //     SR.flipY = false;
    //     SR.color = new Color(1, 1, 1, 1);
    //     CC.enabled = true;
    //     var = 0;
    // }
    // public void StopMove()
    // {
    //     En.constraints = RigidbodyConstraints2D.FreezePosition;
    //     Invoke("ReMove", 10f);
    // }
    // public void ReMove()
    // {
    //     En.constraints = RigidbodyConstraints2D.None;
    //     En.constraints = RigidbodyConstraints2D.FreezeRotation;
    // }
    // public void ActiveHpSlider()
    // {
    //     enemyHpSlider.gameObject.SetActive(true);
    // }
    // public void UnActiveHpSlider()
    // {
    //     enemyHpSlider.gameObject.SetActive(false);
    // }
    // public void DropItem()
    // {
    //     GameObject item = Instantiate(dropItem);
    //     item.transform.position = En.position;
    //     item.SetActive(true);
    // }

    // �÷��̾� ����
    IEnumerator Trace()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(box.position, boxSize, 0);
        foreach (Collider2D CC in collider2Ds)
        {
            if (CC.tag == "Player")
            {
                Next_Move_X = 0;
                Next_Move_Y = 0;
                transform.position = Vector3.MoveTowards(transform.position, CC.transform.position, Time.deltaTime * speed);

                if (currentTime <= 0)
                {
                    GameObject bulletcopy = Instantiate(bullet, bulletPosition.position, transform.rotation);
                    currentTime = coolTime;
                }

            }

        }
        //yield return new WaitForSeconds(5f);
        yield return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(box.position, boxSize);
    }
}
