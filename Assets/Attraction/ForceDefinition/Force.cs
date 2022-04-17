using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;

public abstract class Force : ScriptableObject
{
    protected const float FORCE_CONSTANT = 1f;
    protected const float FORCE_MIN_DISTANCE_CONSTANT = .1f;

    public abstract JobHandle CalculateForces(AffectingForce affectingForce, NativeAffectedByForceArray affectedByForces, NativeArray<Vector3> forceVectors);


}
