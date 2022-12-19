using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
public class FixedEnemy : EnemyController
{
    // Rigidbody2D En; // ��
    // SpriteRenderer SR;
    // Animator animation;
    // BoxCollider2D BC;
    // PlayerMoving player;
    // public int h; // �̵� ���� 

    // public float speed; // �̵� �ӵ�

    // public float EnemyHP; // �� �� ü��
    // public float EnemyCurrentHp; // �� ����ü��
    // public float EnemyAtk; // �� ���ݷ�
    // public float EnemyExp; // �� ����ġ
    // public GameObject hudDamageText; //������ �ؽ�Ʈ
    // public Transform hudPos; //������ �ؽ�Ʈ ��ġ
    // public Slider enemyHpSlider;
    // public GameObject dropItem;
    public Transform AtackBox;

    public Vector2 boxSize;
    public GameObject bullet;
    public Transform bulletPosition;
    public float coolTime; // �� ��Ÿ��
    public float shotTime; // ���� ��Ÿ��(����Ÿ�� >> ������Ÿ��)
    public float shotSpeed;
    private float currentTime;
    private float shotcurrentTime;
    private int shotCount;
    public static float BulletDamage;
    public float BulletDmg;
    //public Slider enemyHpSlider; // ��ü�� ǥ��

    void Awake()
    {
        En = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
        BC = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerMoving>();
        EnemyCurrentHp = EnemyHP;
        enemyHpSlider.gameObject.SetActive(false);
        //Think();
        //Invoke("Think", 5);
        //SR.flipX = h == 1; // ������ ���� �̵��� ���缭 �ٲٱ�
        currentTime = 0;
        shotcurrentTime = 0;
        hp = EnemyCurrentHp;
        EnemyAtkDmg = EnemyAtk;
        BulletDamage = BulletDmg;
        shotCount = 0;
    }
    void Update()
    {
        //if (Mathf.Abs(En.velocity.x) < 0.3)
        //{
        //    animation.SetBool("isrunning", false);
        //}
        //else
        //{
        //    animation.SetBool("isrunning", true);
        //}

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;

        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AtackBox.position, boxSize, 0);
        foreach (Collider2D collider in collider2D)
        {
            if (collider.CompareTag( "Player"))
            {
                Vector2 newbulletPos = collider.transform.position - bulletPosition.position;
                newbulletPos = newbulletPos.normalized; /// from jeongik
                if (currentTime <= 0 || shotCount<=2)
                {
                    if (shotcurrentTime <= 0)
                    {
                        GameObject bulletcopy = Instantiate(bullet, bulletPosition.position, transform.rotation);
                        Rigidbody2D bulletRigid = bulletcopy.GetComponent<Rigidbody2D>();                      
                        bulletRigid.velocity = Mathf.Abs(shotSpeed) * newbulletPos * 1.5f;// from jeongik
                        shotCount++;
                        shotcurrentTime = shotTime;                      
                    }
                    if (shotCount >= 4)
                    {
                        shotCount = 1;
                    }
                    currentTime = coolTime;
                }
                if (newbulletPos.x > 0)
                {
                    if (Mathf.Abs(newbulletPos.x) < Mathf.Abs(newbulletPos.y * (0.8f))) // Top
                    {
                        animation.SetBool("isTurn", true);
                        animation.SetBool("isMiddle", false);
                        animation.SetBool("isUp", true);
                    }
                    else if ((Mathf.Abs(newbulletPos.x) > Mathf.Abs(newbulletPos.y * (0.8f))) && (Mathf.Abs(newbulletPos.x) < Mathf.Abs(newbulletPos.y * (1.6f)))) //Middle
                    {
                        animation.SetBool("isTurn", true);
                        animation.SetBool("isMiddle", true);
                        animation.SetBool("isUp", false);
                    }
                    else  //Idle
                    {
                        animation.SetBool("isTurn", true);
                        animation.SetBool("isMiddle", false);
                        animation.SetBool("isUp", false);
                    }
                }
                else
                {
                    if (Mathf.Abs(newbulletPos.x) < Mathf.Abs(newbulletPos.y * (0.8f)))
                    {
                        animation.SetBool("isTurn", false);
                        animation.SetBool("isMiddle", false);
                        animation.SetBool("isUp", true);
                    }
                    else if ((Mathf.Abs(newbulletPos.x) > Mathf.Abs(newbulletPos.y * (0.8f))) && (Mathf.Abs(newbulletPos.x) < Mathf.Abs(newbulletPos.y * (1.6f))))
                    {
                        animation.SetBool("isTurn", false);
                        animation.SetBool("isMiddle", true);
                        animation.SetBool("isUp", false);
                    }
                    else
                    {
                        animation.SetBool("isTurn", false);
                        animation.SetBool("isMiddle", false);
                        animation.SetBool("isUp", false);
                    }
                }
            }
        }

        shotcurrentTime -= Time.deltaTime;
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
                BC.enabled = false;
                Invoke("DeActive", 1);
                Invoke("UnActiveHpSlider", 1);
                Invoke("DropItem", 1f);
                StopMove();
                exp += EnemyExp;
                // player.CircleEnergy += EnemyExp;
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

        //// �̵�
        //En.velocity = new Vector2(h * speed, En.velocity.y);
        ////Debug.Log(h);

        //// ���� �̵� �� �ٴ� ����
        //if (En.velocity.x > 0)
        //{
        //    Debug.DrawRay(En.position + Vector2.right * 0.3f, Vector2.down, new Color(0, 1, 0));
        //    RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.right * 0.3f, Vector2.down, 1, LayerMask.GetMask("normalfloor"));
        //    //Debug.Log(rayHit.collider);
        //    if (rayHit.collider == null)
        //    {
        //        Turn();
        //    }
        //}

        //// ���� �̵� �� �ٴ� ����
        //else if (En.velocity.x < 0)
        //{
        //    Debug.DrawRay(En.position + Vector2.left * 0.3f, Vector2.down, new Color(0, 1, 0));
        //    RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.left * 0.3f, Vector2.down, 1, LayerMask.GetMask("normalfloor"));
        //    //Debug.Log(rayHit.collider);
        //    if (rayHit.collider == null)
        //    {
        //        Turn();
        //    }
        //}

    }

    void Think()
    {

        //// ������ �ð� ���� �ൿ ���� ��ȭ (���� ���� ����)
        //h = Random.Range(-1, 2);
        //float thinktime = Random.Range(2f, 4f);

        //Invoke("Think", thinktime); // thinktime���� think ��ȣ�� ȸ��

        //if (h != 0)
        //{
        //    SR.flipX = h == 1;
        //}
    }

    void Turn()
    {

        //// �̵� ���� ��ȭ
        //h *= (-1);
        //SR.flipX = h == 1;

        //CancelInvoke(); // �̵� �ٲٸ� �ð� �ʱ�ȭ
        //Invoke("Think", 5);
    }

    // public void DeActive()
    // {
    //     gameObject.SetActive(false);
    // }
    // public void ReturnEnemyColor()
    // {
    //     SR.color = new Color(1, 1, 1, 1);
    // }
    // // �� ��Ȱ
    // public void Revive()
    // {
    //     hp = EnemyHP;
    //     EnemyCurrentHp = EnemyHP;
    //     gameObject.SetActive(true);
    //     SR.flipY = false;
    //     SR.color = new Color(1, 1, 1, 1);
    //     BC.enabled = true;
    // }
    // //�� �׾����� ��ġ f�� ����
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
    //     int percentItem = Random.Range(0, 10);
    //     if (percentItem > 7)
    //     {
    //         GameObject item = Instantiate(dropItem);
    //         item.transform.position = En.position;
    //         //Rigidbody2D itemRigid;
    //         //itemRigid = GetComponent<Rigidbody2D>();
    //         //itemRigid.AddForce(Vector2.up * 50, ForceMode2D.Impulse);
    //         item.SetActive(true);
    //     }
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(AtackBox.position, boxSize);


    }
}
