using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class EnemyBossMove : EnemyController
{
    // Rigidbody2D En; // ��
    // SpriteRenderer SR;
    // Animator animation;
    // public PlayerMoving player;
    // CapsuleCollider2D CC;

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

    //public Slider enemyHpSlider; // ��ü�� ǥ��

    void Awake()
    {
        En = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
        CC = GetComponent<CapsuleCollider2D>();
        EnemyCurrentHp = EnemyHP;
        enemyHpSlider.gameObject.SetActive(false);
        Think();
        Invoke("Think", 5);
        SR.flipX = h == 1; // ������ ���� �̵��� ���缭 �ٲٱ�
        hp = EnemyCurrentHp;
        EnemyAtkDmg = EnemyAtk;
    }
    void Update()
    {
        if (Mathf.Abs(En.velocity.x) < 0.3)
        {
            animation.SetBool("isrunning", false);
        }
        else
        {
            animation.SetBool("isrunning", true);
        }
        //enemyHpSlider.maxValue = EnemyHP;
        //enemyHpSlider.value = EnemyCurrentHp;

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;
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
                Invoke("DeActive", 1);
                Invoke("UnActiveHpSlider", 1);
                Invoke("DropItem", 1f);
                StopMove();
                exp += EnemyExp;
                //player.CircleEnergy += EnemyExp;
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

        // �̵�
        En.velocity = new Vector2(h * speed, En.velocity.y);
        //Debug.Log(h);

        // ���� �̵� �� �ٴ� ����
        if (En.velocity.x > 0)
        {
            Debug.DrawRay(En.position + Vector2.right * 2f, Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.right * 5f, Vector2.down, 4, LayerMask.GetMask("normalfloor","blockedfloor")); //1214 from jeongik
            //Debug.Log(rayHit.collider);
            if (rayHit.collider == null)
            {
                Turn();
            }
        }

        // ���� �̵� �� �ٴ� ����
        else if (En.velocity.x < 0)
        {
            Debug.DrawRay(En.position + Vector2.left * 2f, Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(En.position + Vector2.left * 5f, Vector2.down, 4, LayerMask.GetMask("normalfloor","blockedfloor")); //1214 from jeongik
            //Debug.Log(rayHit.collider);
            if (rayHit.collider == null)
            {
                Turn();
            }
        }

        if ((EnemyCurrentHp >= EnemyHP * 0.35) && (EnemyCurrentHp < EnemyHP * 0.7))
        {
            Phase2();
        }
        else if (EnemyCurrentHp < EnemyHP * 0.35)
        {
            Phase3();
        }

    }

    void Think()
    {

        // ������ �ð� ���� �ൿ ���� ��ȭ (���� ���� ����)
        h = Random.Range(-1, 2);
        float thinktime = Random.Range(2f, 4f);

        Invoke("Think", thinktime); // thinktime���� think ��ȣ�� ȸ��

        if (h != 0)
        {
            SR.flipX = h == 1;
        }
    }

    void Turn()
    {

        // �̵� ���� ��ȭ
        h *= (-1);
        SR.flipX = h == 1;

        CancelInvoke(); // �̵� �ٲٸ� �ð� �ʱ�ȭ
        Invoke("Think", 5);
    }


    //public void EnemyDamaged()
    //{
    //    SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
    //    SR.flipY = true;
    //    h = 0;
    //    CC.enabled = false;
    //    En.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    //    Invoke("DeActive", 3);
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
    //        Invoke("DeActive", 1);
    //        Invoke("UnActiveHpSlider", 1);
    //        Invoke("DropItem", 1.2f);
    //        StopMove();
    //        player.CurrentExp += EnemyExp;
    //        Invoke("Revive", 100);
    //    }
    //    else
    //    {
    //        Invoke("UnActiveHpSlider", 5);
    //        En.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    //        SR.color = new Color(1, 0.7f, 0.02f, 0.5f);
    //        Invoke("ReturnEnemyColor", 1);
    //    }
    //}
    //�� ��Ȱ��ȭ
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
    //     CC.enabled = true;
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

    public void Phase2()
    {
        animation.SetTrigger("Phase2");
    }
    public void Phase3()
    {
        animation.SetTrigger("Phase3");
    }
    // public void DropItem()
    // {
    //     GameObject item = Instantiate(dropItem);
    //     item.transform.position = En.position;
    //     item.SetActive(true);
    // }

}
