using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDriver : MonoBehaviour
{

    private static ForceDriver _instance;

    public static ForceDriver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ForceDriver>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private int forcesAffectorSamples = 10;

    [SerializeField]
    private int numberOfAffected = 0;

    [SerializeField]
    private int numberOfAffecting = 0;

    private List<AffectedByForces> affectedByForcesList = new List<AffectedByForces>();

    private List<AffectingForce> affectingForcesList = new List<AffectingForce>();

    // To start, this is going to simply approximate gravity.
    void Update()
    {
        numberOfAffected = affectedByForcesList.Count;
        numberOfAffecting = affectingForcesList.Count;

        foreach (AffectedByForces affected in affectedByForcesList)
        {
            // Sample X amount of affectors. If there aren't N affectors, don't oversample.
            for (int i = 0; i < forcesAffectorSamples && i < affectedByForcesList.Count; i++)
            {

                // Randomly sample all effecting forces and apply.
                int sampleIndex = Random.Range(0, affectingForcesList.Count);
                AffectingForce affectingForce = affectingForcesList[sampleIndex];

                // Apply this force.
                affectingForce.ApplyForce(affected);
            }
        }
    }

    public int GetCountOfAffectedByForce() {
        return this.affectedByForcesList.Count;
    }

    public void RegisterAffectedByForces(AffectedByForces affectedByForces)
    {
        affectedByForcesList.Add(affectedByForces);
        affectedByForces.OnRegisterableDestroy += () =>
        {
            affectedByForcesList.Remove(affectedByForces);
        };
    }

    public void RegisterAffectingForce(AffectingForce affectsForce)
    {
        affectingForcesList.Add(affectsForce);
        affectsForce.OnRegisterableDestroy += () =>
        {
            affectingForcesList.Remove(affectsForce);
        };
    }
    
}
