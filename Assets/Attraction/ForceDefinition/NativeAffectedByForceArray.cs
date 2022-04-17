using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using System;

public struct NativeAffectedByForceArray
{
    public NativeArray<Vector3> affectedPositions;// = new NativeArray<Vector3>(affectedByForces.Count, Allocator.TempJob);
    public NativeArray<float> affectedForceStrengths;// = new NativeArray<float>(affectedByForces.Count, Allocator.TempJob);
}
