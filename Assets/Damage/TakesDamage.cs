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
    private float damageCooldown = 0f;

    [SerializeField]
    private List<DamageClass> takesDamageFromClasses;
    public List<DamageClass> TakesDamageFromClasses => takesDamageFromClasses;

    private HPObject hpObject;

    private float timeSinceLastDamage = float.MaxValue;

    void Start()
    {
        hpObject = this.GetComponent<HPObject>();
    }

    // Returns true if killed
    public bool Damage(DoesDamage doesDamage)
    {
        timeSinceLastDamage += Time.deltaTime;
        if (takeDamage && timeSinceLastDamage >= damageCooldown)
        {
            PopupManager.Instance.createPopupText(transform.position, doesDamage.DamageAmount.ToString(), Color.white);
            OnDamageTaken(doesDamage);
            timeSinceLastDamage = 0;
            return hpObject.DoDamage(doesDamage.DamageAmount);
        }
        return false;
    }

}
