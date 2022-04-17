using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using System;

[CreateAssetMenu(fileName = "New Force", menuName = "ScriptableObjects/Forces/RepulsiveForce", order = 1)]
public class RepulsiveForce : Force
{
    [SerializeField]
    private float distanceSensitivity = 0.5f;

    public override JobHandle CalculateForces(AffectingForce affectingForce, NativeAffectedByForceArray affectedByForces, NativeArray<int> indicesToCalculate, NativeArray<Vector3> forceVectors)
    {

        AttractiveForceJob job = new AttractiveForceJob
        {
            affectedPositions = affectedByForces.affectedPositions,
            affectedForceStrengths = affectedByForces.affectedForceStrengths,
            indicesToCalculate = indicesToCalculate,
            affectingForcePosition = affectingForce.transform.position,
            affectingForceStrength = affectingForce.ForceStrength,
            deltaTime = Time.deltaTime,
            distanceSensitivity = distanceSensitivity,
            netForces = forceVectors
        };

        JobHandle handle = job.Schedule(indicesToCalculate.Length, 32);
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
        public NativeArray<int> indicesToCalculate;

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
            int indexToCalculate = indicesToCalculate[i];

            // No apply the cumulative force on the affectedForce.
            float distance = Vector3.Distance(affectedPositions[indexToCalculate], affectingForcePosition);

            // Don't wanna be dividing by zero.
            if (distance > FORCE_MIN_DISTANCE_CONSTANT)
            {
                float netForceStrength = -1 * FORCE_CONSTANT * affectedForceStrengths[indexToCalculate] * affectingForceStrength / Mathf.Pow(distance, distanceSensitivity);// Something sorta like gravity.

                Vector3 forceDirection = (affectingForcePosition - affectedPositions[indexToCalculate]).normalized;

                netForces[i] = forceDirection * netForceStrength * deltaTime;
            }
        }
    }
}