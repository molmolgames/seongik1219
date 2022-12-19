using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Enemy;
    private void Start()
    {
        Enemy = this.transform.parent.gameObject;
    }
}
