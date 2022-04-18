using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HPObject))]
public class TakesDamage : MonoBehaviour
{
    [SerializeField]
    private List<DamageClass> takesDamageFromClasses;
    public List<DamageClass> TakesDamageFromClasses => takesDamageFromClasses;

    private HPObject hpObject;

    void Start()
    {
        hpObject = this.GetComponent<HPObject>();
    }

    public void Damage(int damageAmount)
    {
        hpObject.DoDamage(damageAmount);
    }

}
