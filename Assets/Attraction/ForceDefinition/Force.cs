using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Force : ScriptableObject
{
    protected const float FORCE_CONSTANT = 1f;
    protected const float FORCE_MIN_DISTANCE_CONSTANT = .1f;
    public abstract void ApplyForce(AffectingForce affectingForce, AffectedByForces affected);
}
