using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesDirectionOfMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float differenceTolerance = 1f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = this.transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(lastPosition, this.transform.position) > differenceTolerance)
        {
            Quaternion targetRotation = Quaternion.LookRotation((this.transform.position - lastPosition).normalized, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            lastPosition = this.transform.position;
        }
    }
}
