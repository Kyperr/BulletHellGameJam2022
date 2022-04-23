using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    private Color color;

    public void init(string str, Color col)
    {
        color = col;
        text.SetText(str);
        text.color = col;
    }

    void Start()
    {
    }

    public void FadeOut(float time)
    {
        StartCoroutine(DisplayAndFadeOut(time));
    }

    IEnumerator DisplayAndFadeOut(float time)
    {
        float waitTime = 0;
        while (waitTime < time)
        {
            text.color = Color.Lerp(color, Color.clear, waitTime / time);
            waitTime += Time.deltaTime;
            yield return null;
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
