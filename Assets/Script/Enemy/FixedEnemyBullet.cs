using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnemyBullet : DamageController
{
    private void Awake()
    {

    }

    void Start()
    {
        Invoke("DestroyBullet", 5);
    }


    void Update()
    {

        //transform.Translate(transform.right * var * bulletSpeed * Time.deltaTime);
        Invoke("DestroyBullet", 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<DamageController>()?.PlayerOnDamage(FixedEnemy.BulletDamage);
            Rigidbody2D PlayerRigid = collision.GetComponent<Rigidbody2D>();
            PlayerRigid.AddForce(new Vector2(0, 1) * 5, ForceMode2D.Impulse);
            DestroyBullet();
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
