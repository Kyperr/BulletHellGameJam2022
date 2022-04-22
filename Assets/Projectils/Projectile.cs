using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class Projectile : MonoBehaviour
{

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        DestroysProjectileOnHit destroys = other.gameObject.GetComponent<DestroysProjectileOnHit>();
        if (destroys)
        {
            Destroy(this.gameObject);
        }
    }
}
