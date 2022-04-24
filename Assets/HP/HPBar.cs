using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    Image barFilling;

    float changeTime = 0.3f;

    float maxValue;
    float currentValue;

    Transform hpObject;

    [SerializeField]
    Vector3 offset;


    public void init(float maxV, HPObject obj)
    {
        maxValue = currentValue = maxV;
        hpObject = obj.transform;
        transform.SetParent(hpObject.parent);
        updateUI();
    }

    public void update(float v)
    {
        currentValue = v;
        updateUI();
    }

    void updateUI()
    {
        DOTween.To(() => barFilling.fillAmount, x => barFilling.fillAmount = x, currentValue / maxValue, changeTime);

    }
    private void FixedUpdate()
    {
        if (hpObject)
        {

            transform.position = hpObject.position + offset;
        }
    }

    void destroy()
    {
        Destroy(gameObject);
    }
    //Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        //transform.LookAt(transform.position + cam.forward);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
