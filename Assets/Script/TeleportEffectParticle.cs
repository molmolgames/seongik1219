using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffectParticle : MonoBehaviour
{
    ParticleSystem particle;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
