using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIfBlackHoleKill : MonoBehaviour
{

    [SerializeField]
    private int amountToHeal = 1;

    private HPObject playerHealth;

    void Start()
    {
        playerHealth = Player.Instance.GetComponent<HPObject>();

        DoesDamage doesDamage = GetComponent<DoesDamage>();
        if (doesDamage && playerHealth)
        {
            doesDamage.OnDamageDone += (didKill) =>
            {
                if (didKill)
                {
                    playerHealth.DoHealing(amountToHeal);
                    PopupManager.Instance.createPopupText(transform.position, "Heal +" + amountToHeal, Color.green);
                }
            };
        }
    }
}
