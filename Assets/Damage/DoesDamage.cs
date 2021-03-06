using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesDamage : MonoBehaviour
{

    public delegate void OnDamageDoneDelegate(TakesDamage takesDamage, bool didKill);
    public event OnDamageDoneDelegate OnDamageDone = delegate { };

    [SerializeField]
    private DamageClass damageClass;

    [SerializeField]
    private int damageAmount = 1;
    public int DamageAmount => damageAmount;


    void OnTriggerEnter(Collider other)
    {
        TakesDamage takesDamage = other.gameObject.GetComponent<TakesDamage>();
        if (takesDamage && takesDamage.TakesDamageFromClasses.Contains(damageClass))
        {
            OnDamageDone(takesDamage, takesDamage.Damage(this));
        }
    }
}
