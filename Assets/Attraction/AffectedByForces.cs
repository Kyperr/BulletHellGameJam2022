using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Velocity))]
public class AffectedByForces : AnnouncesDestroy
{

    [SerializeField]
    private float forceStrength = 0.0f;
    public float ForceStrength => forceStrength;

    void Start()
    {
        ForceDriver.Instance.RegisterAffectedByForces(this);
    }

}
