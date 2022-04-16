using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowOverShoulder : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float distance = 25;

    [SerializeField]
    private float minDistance = 5;

    [SerializeField]
    private float maxDistance = 60;

    // When the scrollwheel changes, how far will we zoome in our out?
    [SerializeField]
    private float distanceChangeAmount = 5;

    [SerializeField]
    private Vector3 viewingPositionVector = new Vector3(0, .5f, -.5f);

    [SerializeField]
    private Vector3 viewingAngleVector = new Vector3(130f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
        // float x = target.transform.position.x;
        // float z = target.transform.position.z;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance += -1 * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * distanceChangeAmount;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        Vector3 positionDif = viewingPositionVector * distance;
        transform.position = target.transform.position + positionDif;
        transform.rotation = Quaternion.Euler(viewingAngleVector);

        // transform.position = Vector3.Lerp(transform.position, new Vector3(x, height, z), Time.deltaTime * 5);
    }
}
