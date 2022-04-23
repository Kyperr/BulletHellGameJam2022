using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    string text;
    Color color;
    public void init(string str, Color col)
    {
        text = str;
        color = col;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = text;
        GetComponentInChildren<Text>().color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
