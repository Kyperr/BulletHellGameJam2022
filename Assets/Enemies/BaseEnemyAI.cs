using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target))]
public class BaseEnemyAI : MonoBehaviour
{

    private const float DESIRED_POSITION_THRESHOLD = 5.0f;

    // Returns true if at desired position.
    protected bool MoveToDesiredPosition(Vector3 desiredPosition, float moveSpeed)
    {
        Vector3 nextPos = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * moveSpeed);

        transform.LookAt(nextPos, Vector3.up);
        Debug.DrawLine(this.transform.position, desiredPosition);

        this.transform.position = nextPos;

        if (Vector3.Distance(this.transform.position, desiredPosition) < DESIRED_POSITION_THRESHOLD)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}