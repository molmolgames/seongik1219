using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class EnemyController : DamageController
{
    [HideInInspector] public Rigidbody2D En; // 적
    [HideInInspector] public SpriteRenderer SR; // 스프라이트렌더러
    [HideInInspector] public BoxCollider2D BC;
    [HideInInspector] public Animator animation; //애니메이션
    public PlayerMoving player; //플레이어
    [HideInInspector] public CapsuleCollider2D CC; //캡슐콜라이더
    [HideInInspector] public int h; // 이동 방향
    public float speed; // 이동 속도
    public float EnemyHP; // 적 총 체력
    [HideInInspector] public float EnemyCurrentHp; //적 현재 체력
    public float EnemyAtk; // 적 공격력
    public float EnemyExp; // 적 경험치
    public GameObject hudDamageText; //데미지 텍스트
    public Transform hudPos; //데미지 텍스트 위치
    public Slider enemyHpSlider; //적 체력바
    public GameObject dropItem; //드랍아이템
    public GameObject dropMoneyObject; // money object
    [HideInInspector] public int var = 0;
    public int droppercent; //드랍아이템확률
    public int dropMoney; // monster drop money amount 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // enemyHpSlider.maxValue = EnemyHP; //적최대체력
        // enemyHpSlider.value = EnemyCurrentHp; //적현재체력값
        // enemyHpSlider.transform.position = hudPos.position; //적체력표시위치
 
        // if (EnemyCurrentHp != hp && EnemyCurrentHp > 0) //적체력인터렉션
        // {
        //     float dmg = EnemyCurrentHp - hp;
        //     GameObject hudText = Instantiate(hudDamageText);
        //     hudText.transform.position = hudPos.position;
        //     hudText.GetComponent<DamageText>().damage = (int)dmg;
        //     EnemyCurrentHp = hp;

        //     ActiveHpSlider();

        //     if (EnemyCurrentHp <= 0)
        //     {
        //         SR.color = new Color(1, 0, 1, 0.5f);
        //         SR.flipY = true;
        //         CC.enabled = false;
        //         Invoke("DeActive", 1);
        //         Invoke("UnActiveHpSlider", 1);
        //         Invoke("DropItem", 1.2f);
        //         StopMove();
        //         player.CurrentExp += EnemyExp;
        //         Invoke("Revive", 10);
        //     }
        //     else
        //     {
        //         Invoke("UnActiveHpSlider", 5);
        //         En.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        //         SR.color = new Color(1, 0.9f, 0.02f, 0.5f);
        //         Invoke("ReturnEnemyColor", 1);
        //     }
        // }
    }
     public void DeActive() //적비활성화함수 추후 수정
    {
        gameObject.SetActive(false);
    }
    public void StopMove() //움직임멈춤
    {
        En.constraints = RigidbodyConstraints2D.FreezePosition;
        Invoke("ReMove", 10f);
    }
    public void ActiveHpSlider() //체력바활성함수
    {
        enemyHpSlider.gameObject.SetActive(true);
    }
    public void UnActiveHpSlider() //체력바비활성화함수
    {
        enemyHpSlider.gameObject.SetActive(false);
    }
    public void ReturnEnemyColor() //적 색깔 회복함수
    {
        SR.color = new Color(1, 1, 1, 1);
    }
    public void Revive() //적 부활 함수
    {
        hp = EnemyHP;
        EnemyCurrentHp = EnemyHP;
        gameObject.SetActive(true);
        SR.flipY = false;
        SR.color = new Color(1, 1, 1, 1);
        CC.enabled = true;
        var = 0;
    }
    public void ReMove() //제거함수
    {
        En.constraints = RigidbodyConstraints2D.None;
        En.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public IEnumerator DropItem(int droppercent) //아이템드랍함수
    {
        yield return new WaitForSeconds(0.2f);
        int percentItem = Random.Range(0, 101);
        if (percentItem <= droppercent)
        {
            GameObject item = Instantiate(dropItem);
            item.transform.position = En.position;
            Rigidbody2D itemRigid = item.GetComponent<Rigidbody2D>();
            Vector2 vec = new Vector2(0, 7);
            itemRigid.AddForce(vec, ForceMode2D.Impulse);
            item.SetActive(true);
        }   
        yield break;
    }
    public void DropMoney()
    {
        int range = Random.Range(1, 11);
        GameObject MoneyObject = Instantiate(dropMoneyObject);
        MoneyData moneyData = MoneyObject.GetComponent<MoneyData>();
        moneyData.moneyAmount = dropMoney* range;
        MoneyObject.transform.position = En.position;
        Rigidbody2D MoneyObjectRigid = MoneyObject.GetComponent<Rigidbody2D>();
        Vector2 vec = new Vector2(0, 7);
        MoneyObjectRigid.AddForce(vec, ForceMode2D.Impulse);
        MoneyObject.SetActive(true);

    }
}