using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using System;

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

    public override JobHandle CalculateForces(AffectingForce affectingForce, NativeAffectedByForceArray affectedByForces, NativeArray<Vector3> forceVectors)
    {

        AttractiveForceJob job = new AttractiveForceJob
        {
            affectedPositions = affectedByForces.affectedPositions,
            affectedForceStrengths = affectedByForces.affectedForceStrengths,
            affectingForcePosition = affectingForce.transform.position,
            affectingForceStrength = affectingForce.ForceStrength,
            deltaTime = Time.deltaTime,
            distanceSensitivity = distanceSensitivity,
            netForces = forceVectors
        };

        JobHandle handle = job.Schedule(affectedByForces.affectedPositions.Length, 32);
        return handle;
    }

    [BurstCompile]
    public struct AttractiveForceJob : ForceJob
    {

        [ReadOnly]
        public NativeArray<Vector3> affectedPositions;

        [ReadOnly]
        public NativeArray<float> affectedForceStrengths;

        [ReadOnly]
        public Vector3 affectingForcePosition;

        [ReadOnly]
        public float affectingForceStrength;

        [ReadOnly]
        public float deltaTime;

        [ReadOnly]
        public float distanceSensitivity;

        public NativeArray<Vector3> netForces;

        public void Execute(int i)
        {

            // No apply the cumulative force on the affectedForce.
            float distance = Vector3.Distance(affectedPositions[i], affectingForcePosition);

            // Don't wanna be dividing by zero.
            if (distance > FORCE_MIN_DISTANCE_CONSTANT)
            {
                float netForceStrength = FORCE_CONSTANT * affectedForceStrengths[i] * affectingForceStrength / Mathf.Pow(distance, distanceSensitivity);// Something sorta like gravity.

                Vector3 forceDirection = (affectingForcePosition - affectedPositions[i]).normalized;

                netForces[i] = forceDirection * netForceStrength * deltaTime;
            }
        }
    }
}
