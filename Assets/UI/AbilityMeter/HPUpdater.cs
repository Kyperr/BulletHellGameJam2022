using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUpdater : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private HPObject hpObject;

    private Slider slider;

    private Text text;

    void Start()
    {
        hpObject = player.GetComponent<HPObject>();
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        slider.value = hpObject.CurrentHP;
        slider.maxValue = hpObject.MaxHP;
        text.text = Mathf.Max(hpObject.CurrentHP, 0) + "/" + hpObject.MaxHP + " HP";
    }
}
