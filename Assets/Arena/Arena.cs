using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    Transform arenaTransform;

    [SerializeField]
    float minimalRadius = 0.3f;
    [SerializeField]
    float radiusChangeSpeed = 1f;

    [ContextMenu("Set Random Arena Radius")]

    public void setRandomArenaRadius()
    {
        float r = Random.Range(minimalRadius, 1);
        setArenaRadius(r);
    }
    public void setArenaRadius(float r)
    {
        r = Mathf.Max(minimalRadius, r);
        var diffR = arenaTransform.localScale.x - r;
        arenaTransform.DOScale(new Vector3(r, arenaTransform.localScale.y, r), diffR*radiusChangeSpeed).SetEase(Ease.Linear);
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
