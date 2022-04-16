using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Force", menuName = "ScriptableObjects/Forces/RepulsiveForce", order = 1)]
public class RepulsiveForce : Force
{

    [SerializeField]
    private float distanceSensitivity = 2;

    public override void ApplyForce(AffectingForce affectingForce, AffectedByForces affected)
    {
        Vector3 affectedPosition = affected.transform.position;
        float affectedForceStrength = affected.ForceStrength;

        Vector3 affectingForcePosition = affectingForce.transform.position;
        float affectingForceStrength = affectingForce.ForceStrength;

        // No apply the cumulative force on the affectedForce.
        float distance = Vector3.Distance(affectedPosition, affectingForcePosition);

        // Don't wanna be dividing by zero.
        if (distance > FORCE_MIN_DISTANCE_CONSTANT)
        {
            float netForceStrength = -1 * FORCE_CONSTANT * affectedForceStrength * affectingForceStrength / Mathf.Pow(distance, distanceSensitivity);

            Vector3 forceDirection = (affectingForcePosition - affectedPosition).normalized;

            Vector3 netForce = forceDirection * netForceStrength * Time.deltaTime;

            affected.GetComponent<Velocity>().AffectVelocity(velocity =>
            {
                return velocity + netForce;
            });
        }
    }
}
