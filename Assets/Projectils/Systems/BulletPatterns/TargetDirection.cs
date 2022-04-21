using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDirection : MonoBehaviour
{
    private const float DIFF_THRESHOLD = 1.0f;

    [SerializeField]
    private Vector3 targetDirection;
    public Vector3 Direction => targetDirection;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {

        if (Vector3.Distance(lastPosition, this.transform.position) > DIFF_THRESHOLD)
        {
            targetDirection = (this.transform.position - lastPosition).normalized;

            lastPosition = transform.position;
        }
    }
}
