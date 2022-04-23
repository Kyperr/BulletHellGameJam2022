using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HPObject))]
public class TakesDamage : MonoBehaviour
{

    public delegate void OnDamageTakenDelegate(DoesDamage doesDamage);
    public event OnDamageTakenDelegate OnDamageTaken = delegate { };


    [SerializeField]
    private bool takeDamage = true;
    public bool TakeDamage { get => takeDamage; set => takeDamage = value; }


    [SerializeField]
    private List<DamageClass> takesDamageFromClasses;
    public List<DamageClass> TakesDamageFromClasses => takesDamageFromClasses;

    private HPObject hpObject;

    void Start()
    {
        hpObject = this.GetComponent<HPObject>();
    }

    public void Damage(DoesDamage doesDamage)
    {
        if (takeDamage)
        {
            PopupManager.Instance.createPopupText(transform.position, doesDamage.DamageAmount.ToString(), Color.white);
            hpObject.DoDamage(doesDamage.DamageAmount);
            OnDamageTaken(doesDamage);
        }
    }

}
