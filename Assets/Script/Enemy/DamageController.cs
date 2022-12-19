using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    protected float hp;
    [HideInInspector] public float EnemyAtkDmg;
    protected float exp;
    int SlowStack;
    public virtual void EnemyOnDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerMoving>().exp += exp;
            Invoke("DestroyObject", 1.2f);
        }
    }
    public virtual void EnemyOnSlowDamage(float damage)  //슬로우스킬피해받았을때
    {
        hp -= damage;
        StartCoroutine(SlowSpeed());
        StartCoroutine(SlowColor());
        hp -= damage;
        if (hp <= 0)
        {
            Invoke("DestroyObject",1.2f);
        }
    }
    public virtual void PlayerOnDamage(float damage)
    {
        hp -= damage;
        //if (hp <= 0)
        //{
        //    DestroyObject();
        //}
    }
    public virtual void DestroyObject()
    {
        Destroy(gameObject);
    }
    protected void CommonDestroyFunction()
    {
        //GameObject obj = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        //if (isEnemy == true)
        //{
        //    GameManager.Instance?.RemoveEnemy(this);
        //    GameManager.TargetController?.RemoveTargetUI(this);
        //    GameManager.WeaponController?.ChangeTarget();

        //    GameManager.PlayerAircraft.OnScore(objectInfo.Score);
        //}
    }
    IEnumerator SlowSpeed()
    {
        if (SlowStack == 0)
        {
            gameObject.GetComponent<EnemyController>().speed = gameObject.GetComponent<EnemyController>().speed * 0.5f;
        }
        SlowStack++;
        yield return new WaitForSeconds(3f);
        yield return new WaitForSeconds(SlowStack * 0.5f);
        StartCoroutine(ReturnSpeed());

        yield break;
    }
    IEnumerator ReturnSpeed()
    {
        if (SlowStack >= 1)
        {
            gameObject.GetComponent<EnemyController>().speed = gameObject.GetComponent<EnemyController>().speed * (2f);
            SlowStack = 0;
        }

        yield break;
    }
    IEnumerator SlowColor()
    {
        int i = 0;
        while (i < (3 + SlowStack * 0.5f))
        {
            gameObject.GetComponent<EnemyController>().SR.color = new Color(0, 0, 0.2f+i*0.2f, 1);
            i++;
            yield return new WaitForSeconds(1f);
        }
        gameObject.GetComponent<EnemyController>().SR.color = new Color(1, 1, 1, 1);
        yield break;
    }
}