using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowYLocked : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float y;

    // Update is called once per frame
    void Update()
    {
        float x = target.transform.position.x;
        float z = target.transform.position.z;

        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), Time.deltaTime * 5);
    }
}
