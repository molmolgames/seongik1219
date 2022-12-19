using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
public class MonsterBox : MonoBehaviour
{
    Rigidbody2D En; // 적
    SpriteRenderer SR; 
    // Animator animation;
    public PlayerMoving player;
    BoxCollider2D BC;
    public float EnemyHP; // 적 총 체력
    public float EnemyCurrentHp; // 적 현재체력
    // Start is called before the first frame update
    public GameObject hudDamageText; //데미지 텍스트
    public Transform hudPos; //데미지 텍스트 위치
    public Slider enemyHpSlider;
    public GameObject dropItem;
    void Start()
    {
        
    }
    void Awake() 
    {
        En = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        // animation = GetComponent<Animator>();
        BC = GetComponent<BoxCollider2D>();
        EnemyCurrentHp = EnemyHP;
        //enemyHpSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Mathf.Abs(En.velocity.x) < 0.3)
        // {
        //     animation.SetBool("isrunning", false);
        // }
        // else
        // {
        //     animation.SetBool("isrunning", true);
        // }
        //enemyHpSlider.maxValue = EnemyHP;
        //enemyHpSlider.value = EnemyCurrentHp;

        enemyHpSlider.maxValue = EnemyHP;
        enemyHpSlider.value = EnemyCurrentHp;
        enemyHpSlider.transform.position = hudPos.position;
    }
    public void EnemyDamaged(float damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = (int)damage;
        EnemyCurrentHp = EnemyCurrentHp - damage;

        ActiveHpSlider();

        if (EnemyCurrentHp <= 0)
        {
            SR.color = new Color(1, 0, 1, 0.5f);
            SR.flipY = true;
            BC.enabled = false;
            Invoke("DeActive", 1);
            Invoke("UnActiveHpSlider", 1);
            Invoke("DropItem", 1.2f);
            Invoke("Revive", 10);
        }
        else
        {
            Invoke("UnActiveHpSlider", 5);
            En.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
            Invoke("ReturnEnemyColor", 1);
        }
    }
    public void DeActive()
    {
        gameObject.SetActive(false);
    }
    public void ReturnEnemyColor()
    {
        SR.color = new Color(1, 1, 1, 1);
    }
     public void ActiveHpSlider()
    {
        enemyHpSlider.gameObject.SetActive(true);
    }
    public void UnActiveHpSlider()
    {
        enemyHpSlider.gameObject.SetActive(false);
    }
    public void DropItem()
    {
        int percentItem = Random.Range(0, 10);
        if (percentItem > 7)
        {
            GameObject item = Instantiate(dropItem);
            item.transform.position = En.position;
            //Rigidbody2D itemRigid;
            //itemRigid = GetComponent<Rigidbody2D>();
            //itemRigid.AddForce(Vector2.up * 50, ForceMode2D.Impulse);
            item.SetActive(true);
        }
    }
}



