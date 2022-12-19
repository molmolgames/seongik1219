using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerSkill : MonoBehaviour
{
    PlayerMoving player;
    public bool isSkill;
    public float coolTime;
    public float currentTime;
    public Image SkillImage;
    public Transform pos;
    Animator animator;
    int count;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMoving>();
        isSkill = false;
        currentTime = 0;
        count = 0;
    }
    private void Update()
    {
        transform.position = pos.position;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        SkillImage.fillAmount = (currentTime / coolTime);
        if (count >= 1)
        {
            animator.SetTrigger("SkillOn");
            currentTime = coolTime;
            isSkill = false;
            count = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isSkill == true || Input.GetKey(KeyCode.Q))
        {
            if (currentTime <= 0)
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    Debug.Log("Skillhit");
                    collision.gameObject.GetComponent<DamageController>()?.EnemyOnDamage(PlayerMoving.SkillDamage);
                }
                count++;
            }
        }
    }
    //public void OnAttack(Transform Enemy)

    //{
    //    EnemyMove enemymove = Enemy.GetComponent<EnemyMove>();
    //    enemymove.EnemyDamaged(PlayerMoving.SkillDamage);

    //}
    public void SkillButton()
    {
        isSkill = true;

        Invoke("SkillFalse", 0.1f);
    }
    void SkillFalse()
    {
        isSkill = false;
    }
}
