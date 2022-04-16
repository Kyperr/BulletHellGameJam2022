using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasingAffectingForce : AffectingForce
{

    [SerializeField]
    private int minForceStrength = 15;

    [SerializeField]
    private int maxForceStrength = 100;

    [SerializeField]
    private float multiplier = 1.5f;

    private ForceDriver forceDriver;

    public override void Start()
    {
        base.Start();
        forceDriver = ForceDriver.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        this.forceStrength = Mathf.Clamp(forceDriver.GetCountOfAffectedByForce() * multiplier, minForceStrength, maxForceStrength);
    }
}
