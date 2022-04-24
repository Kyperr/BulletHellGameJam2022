using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleCooldownBar : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private SimpleControls simpleControls;

    private Image image;

    void Start()
    {

        simpleControls = player.GetComponent<SimpleControls>();
        image = GetComponent<Image>();

    }

    void Update()
    {
        image.fillAmount = simpleControls.GetAltCooldown();
    }
}
