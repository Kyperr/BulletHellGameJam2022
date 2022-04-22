using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System;
using Unity.Collections;

public class AffectingForce : AnnouncesDestroy
{

    [SerializeField]
    protected float forceStrength = 0.0f;
    public float ForceStrength { get => forceStrength; set => forceStrength = value; }

    [SerializeField]
    private Force force;

    public virtual void Start()
    {
        ForceDriver.Instance.RegisterAffectingForce(this);
    }

    public JobHandle CalculateForces(NativeAffectedByForceArray affectedByForces, NativeArray<Vector3> forceVectors)
    {
        return force.CalculateForces(this, affectedByForces, forceVectors);
    }

}
