using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    public GameObject GetTarget()
    {
        return target;
    }
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
