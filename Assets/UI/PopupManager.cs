using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : Singleton<PopupManager>
{
    public GameObject popupTextPrefab;
    public float jumpLeftRightRange = 10;
    public float jumpHeight = -10;
    public float jumpForce = 10;
    public float stayTime = 2;
    public GameObject createPopupText(Vector3 p, string str, Color col, float scale = 1, float time = -1)
    {
        var go = Instantiate(popupTextPrefab, p, popupTextPrefab.transform.rotation);
        if (time == -1)
        {
            time = stayTime;
        }
        go.GetComponent<Transform>().DOJump(p + new Vector3(Random.Range(-jumpLeftRightRange, jumpLeftRightRange), 0, jumpHeight), jumpForce, 1, time);
        go.GetComponent<PopupText>().init(str, col);
        go.GetComponentInChildren<Text>().DOFade(0, time);
        go.transform.localScale = go.transform.localScale * scale;
        Destroy(go, time);
        return go;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
