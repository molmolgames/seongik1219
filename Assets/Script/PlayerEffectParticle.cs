using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffectParticle : MonoBehaviour
{
    public Transform pos;
    void Update()
    {
        transform.position = pos.transform.position;
    }
}
