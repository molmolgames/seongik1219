using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseFloor : MonoBehaviour
{
    Animator animation;
    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animation.SetTrigger("Hit");
            Invoke("DestroyFloor", 1.5f);
        }
    }
    private void DestroyFloor()
    {
        Destroy(gameObject);
    }
}
