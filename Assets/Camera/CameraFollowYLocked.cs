using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowYLocked : MonoBehaviour
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
    private float lerpSpeed = 10f;

    private void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            distance += -1 * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")) * distanceChangeAmount;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float x = target.transform.position.x;
        float y = target.transform.position.y + distance;
        float z = target.transform.position.z;

        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), Time.deltaTime * lerpSpeed);
    }
}
