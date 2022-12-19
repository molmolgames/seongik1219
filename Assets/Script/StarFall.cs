using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFall : MonoBehaviour
{
    public float bulletDamage;
    private void Awake()
    {

    }

    void Start()
    {
        Invoke("DestroyBullet", 5);
        
    }


    void Update()
    {
        Invoke("DestroyBullet", 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<DamageController>()?.PlayerOnDamage(bulletDamage);
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
