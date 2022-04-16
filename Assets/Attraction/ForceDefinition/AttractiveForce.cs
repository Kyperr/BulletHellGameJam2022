using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Calculating true Forces would be unavoidably n^2 for something like bullets.

    Will be attempting a solution that aims to approximate this behavior by sampling the affectsForcess once per update.
    This should give us a *rough* approximate to something like gravity.

*/
[CreateAssetMenu(fileName = "New Force", menuName = "ScriptableObjects/Forces/AttractiveForce", order = 1)]
public class AttractiveForce : Force
{
    [SerializeField]
    private float distanceSensitivity = 0.5f;

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
            float netForceStrength = FORCE_CONSTANT * affectedForceStrength * affectingForceStrength / Mathf.Pow(distance, distanceSensitivity);// Something sorta like gravity.

            Vector3 forceDirection = (affectingForcePosition - affectedPosition).normalized;

            Vector3 netForce = forceDirection * netForceStrength * Time.deltaTime;

            affected.GetComponent<Velocity>().AffectVelocity(velocity =>
            {
                return velocity + netForce;
            });
        }
    }
}
