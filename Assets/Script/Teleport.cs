using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject toObject;
    public ParticleSystem Particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetObject = collision.gameObject;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(TeleportRoutine());
        }
    }
    IEnumerator TeleportRoutine()
    {
        yield return null;
        GameObject go = Instantiate(Particle).gameObject;
        go.transform.position = toObject.transform.position;
        targetObject.transform.position = toObject.transform.position;
        yield return new WaitForSeconds(0.5f);
    }
}
