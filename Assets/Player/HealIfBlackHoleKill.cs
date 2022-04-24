using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIfBlackHoleKill : MonoBehaviour
{

    [SerializeField]
    private int baseRangeAmountToHeal;

    private HPObject playerHealth;

    void Start()
    {
        playerHealth = Player.Instance.GetComponent<HPObject>();

        DoesDamage doesDamage = GetComponent<DoesDamage>();
        if (doesDamage && playerHealth)
        {
            doesDamage.OnDamageDone += (takesDamage, didKill) =>
            {
                if (didKill)
                {
                    float amountToHeal = baseRangeAmountToHeal;
                    AmountToHealFromBlackHole amount = takesDamage.GetComponent<AmountToHealFromBlackHole>();
                    if (amount)
                    {
                        amountToHeal = amount.AmountToHeal;
                    }
                    playerHealth.DoHealing(amountToHeal);
                    PopupManager.Instance.createPopupText(transform.position, "Heal +" + amountToHeal, Color.green);
                }
            };
        }
    }
}
