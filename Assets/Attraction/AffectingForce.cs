using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectingForce : AnnouncesDestroy
{

    [SerializeField]
    protected float forceStrength = 0.0f;
    public float ForceStrength => forceStrength;

    [SerializeField]
    private Force force;

    public virtual void Start()
    {
        ForceDriver.Instance.RegisterAffectingForce(this);
    }

    public void ApplyForce(AffectedByForces affected)
    {
        if (force != null)
        {
            force.ApplyForce(this, affected);
        }
    }

}
