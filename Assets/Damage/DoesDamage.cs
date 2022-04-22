using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesDamage : MonoBehaviour
{

    [SerializeField]
    private DamageClass damageClass;

    [SerializeField]
    private int damageAmount = 1;


    void OnTriggerEnter(Collider other)
    {
        TakesDamage takesDamage = other.gameObject.GetComponent<TakesDamage>();
        if (takesDamage && takesDamage.TakesDamageFromClasses.Contains(damageClass))
        {
            takesDamage.Damage(damageAmount);
        }
    }
}
