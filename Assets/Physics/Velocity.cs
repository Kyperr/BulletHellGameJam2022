using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Velocity : MonoBehaviour
{
    [SerializeField]
    private Vector3 velocity = Vector3.zero; // Per second.

    [SerializeField]
    private Vector3 maxVelocity = Vector3.zero; // Per second.

    [SerializeField]
    private Boolean xLocked = false;
    [SerializeField]
    private Boolean yLocked = false;
    [SerializeField]
    private Boolean zLocked = false;

    [SerializeField]
    private Vector3 lockedCoordinates = Vector3.zero; // Per second.

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void AffectVelocity(Func<Vector3, Vector3> doWithVelocity)
    {
        velocity = doWithVelocity(velocity);
        // velocity = Vector3.Min(velocity, maxVelocity);
    }

    void Update()
    {
        // Divide the velocity by a fraction of a second since the velocity is in terms of a single second.
        Vector3 velocityToApply = this.velocity * Time.deltaTime;
        Vector3 newPos = transform.position + velocityToApply;

        if (xLocked)
        {
            newPos.x = lockedCoordinates.x;
        }
        if (yLocked)
        {
            newPos.y = lockedCoordinates.y;
        }
        if (zLocked)
        {
            newPos.z = lockedCoordinates.z;
        }

        transform.position = newPos;
    }
}
