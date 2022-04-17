using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTemplar : MonoBehaviour
{

    private const float DISTANCE_THRESHOLD = 1.0f;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float desiredDistance = 10f;

    [SerializeField]
    private float circlingAngle = 30f;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float circlingSpeed = 30;

    private bool rotating = true;

    private float desiredAngle = 0;


    void Start()
    {
        desiredAngle = Random.Range(0, 360);
    }

    void Update()
    {

        UpdateDesiredAngle();

        GetToDesiredDistance();

        // 1) Enemy should circle the player for N seconds
        // 2) After N seconds, stop, turn and face player.
        // 3) Shoot bullets towards player and wait for X amount of seconds (configurable)
        // 4) Continue circling the player, randomly choose clockwise or counter clockwise.
    }

    private void UpdateDesiredAngle()
    {
        desiredAngle += Time.deltaTime * circlingSpeed;
    }

    private void GetToDesiredDistance()
    {
        Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * desiredAngle), 0, Mathf.Sin(Mathf.Deg2Rad * desiredAngle));
        Vector3 desiredPosition = this.target.transform.position + (directionalVector * desiredDistance);

        transform.LookAt(desiredPosition, Vector3.up);

        this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
    }

}
