using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class CircleAttack : MonoBehaviour
{
    SpriteRenderer spriteRender;
    Rigidbody2D rigid;
    public GameManger gameManger;
    public EnemyController enemyController;
    
    float EnemyDamage;
    bool is_Slow; //


    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();   
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     is_Slow = gameManger.isSlow;
    //     if (is_Slow == true)
    //     {
    //         spriteRender.color = new Color(0, 0, 1, 1);
    //     }
    //     else
    //     {
    //         spriteRender.color = new Color(1, 1, 1, 1);
    //     }
    // }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            CircleController.currentTime += 1f;//1214 from jeongik
            CircleController.CircleControl = true;//1207 from jeongik

            if (is_Slow == true)
            {
                collision.gameObject.GetComponent<DamageController>()?.EnemyOnSlowDamage(PlayerMoving.TotalPlayerDamage);                
            }
            else
            {
                collision.gameObject.GetComponent<DamageController>()?.EnemyOnDamage(PlayerMoving.TotalPlayerDamage);
            }
        }
    }  
    void OnTriggerEnter2D(Collider2D collision) //1207 from jeongik
    {

        if (collision.gameObject.tag == "HitBox")
        {
            Debug.Log(collision.gameObject.tag);
            HitBox enemy = collision.gameObject.GetComponent<HitBox>();
            GameObject enemycollision = enemy.Enemy;
            CircleController.currentTime += 1f;//1214 from jeongik
            CircleController.CircleControl = true;

            if (is_Slow == true)
            {
                enemycollision.GetComponent<DamageController>()?.EnemyOnSlowDamage(PlayerMoving.TotalPlayerDamage);                
            }
            else
            {
                enemycollision.GetComponent<DamageController>()?.EnemyOnDamage(PlayerMoving.TotalPlayerDamage);
            }
        }
    }  
}
