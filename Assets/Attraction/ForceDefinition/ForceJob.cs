using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public interface ForceJob : IJobParallelFor
{
    NativeArray<Vector3> NetForces { get; }
}
