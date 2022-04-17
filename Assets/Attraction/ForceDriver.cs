using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;

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

    /*
        Instead of n^2 operations, each force will affect completely random affected objects.
        Instead of being accurate with horrible performance, we are being approximate with reasonable performance.
    */
    [SerializeField]
    private int sampleCount = 0;
    private int effectiveSampleCount = 0;

    [SerializeField]
    private float gravityIntensityModifier = 1.0f;

    [SerializeField]
    private int numberOfAffected = 0;

    [SerializeField]
    private int numberOfAffecting = 0;

    private List<AffectedByForces> affectedByForcesList = new List<AffectedByForces>();

    private List<AffectingForce> affectingForcesList = new List<AffectingForce>();

    // To start, this is going to simply approximate gravity.
    void Update()
    {
        if (affectedByForcesList.Count == 0)
        {
            return;
        }

        effectiveSampleCount = Math.Min(affectedByForcesList.Count, sampleCount);

        numberOfAffected = affectedByForcesList.Count;
        numberOfAffecting = affectingForcesList.Count;

        // The native list of jobhandles to complete together at the end.
        NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(affectingForcesList.Count, Allocator.TempJob);

        // These two match up to tell us which affectedByForce the force should apply to.
        List<NativeArray<int>> affectedIndiciesList = new List<NativeArray<int>>(affectingForcesList.Count);
        List<NativeArray<Vector3>> forceVectorList = new List<NativeArray<Vector3>>(affectingForcesList.Count);

        // Some intialization of variables.
        AffectingForce affectingForce;
        NativeArray<Vector3> forceVectors;
        NativeArray<int> affectedIndices;
        int randomIndex;

        // Build the native affectedArray
        NativeAffectedByForceArray nativeAffectedByForceArray = BuildNativeAffectedArray(affectedByForcesList);

        // Iterate over the affecting forces.
        for (int x = 0; x < affectingForcesList.Count; x++)
        {
            // The affecting force for this iteration
            affectingForce = affectingForcesList[x];

            // Choose a random sampling of things to affect and record the indices to be changed.
            affectedIndices = new NativeArray<int>(effectiveSampleCount, Allocator.TempJob);
            affectedIndiciesList.Add(affectedIndices);
            for (int i = 0; i < effectiveSampleCount; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, affectedByForcesList.Count);
                affectedIndices[i] = randomIndex;
            }

            // The native array to get populated
            forceVectors = new NativeArray<Vector3>(effectiveSampleCount, Allocator.TempJob);
            forceVectorList.Add(forceVectors);

            // Choose which ones to sample this frame. Add the random index to a list so we can use it as a reference later.
            List<AffectedByForces> sampledList = new List<AffectedByForces>(effectiveSampleCount);
            jobs[x] = affectingForce.CalculateForces(nativeAffectedByForceArray, affectedIndiciesList[x], forceVectors);
        }

        // COMPLETE ALL THE JOBS
        JobHandle.CompleteAll(jobs);
        jobs.Dispose();
        nativeAffectedByForceArray.affectedPositions.Dispose();
        nativeAffectedByForceArray.affectedForceStrengths.Dispose();

        // Now apply all of the calculated forces.
        ApplyCalculatedForces(affectedIndiciesList, forceVectorList);


        // Now clean up this massive mess
        foreach (NativeArray<Vector3> nativeContainer in forceVectorList)
        {
            nativeContainer.Dispose();
        }
        foreach (NativeArray<int> nativeContainer in affectedIndiciesList)
        {
            nativeContainer.Dispose();
        }

    }

    private void ApplyCalculatedForces(List<NativeArray<int>> affectedIndiciesList, List<NativeArray<Vector3>> forceVectorList)
    {

        // A cache so we are N^2'ing GetComponents
        Dictionary<int, Rigidbody> rigidBodyCache = new Dictionary<int, Rigidbody>();

        //Init some vars
        NativeArray<int> affectedIndices;
        NativeArray<Vector3> forceVectors;
        int affectedIndex;
        AffectedByForces affected;
        Vector3 vectorForce;

        // For each effecting force, there will be a list of affected indicies and forceVectors
        for (int i = 0; i < affectingForcesList.Count; i++)
        {
            affectedIndices = affectedIndiciesList[i];
            forceVectors = forceVectorList[i];

            // For each affected index, get the force vector and apply to that rigid body.
            for (int x = 0; x < affectedIndices.Length; x++)
            {
                // This affected, index-proxied by the affectedIndices array,
                affectedIndex = affectedIndices[x];

                //The force for this affected entity.
                vectorForce = forceVectors[x] * gravityIntensityModifier;

                // Get the rigid body and add the force.
                Rigidbody rb;
                if (rigidBodyCache.TryGetValue(affectedIndex, out rb))
                {
                    rb.AddForce(vectorForce);
                }
                else
                {
                    affected = affectedByForcesList[affectedIndex];
                    rb = affected.GetComponent<Rigidbody>();
                    rigidBodyCache[affectedIndex] = rb;
                    rb.AddForce(vectorForce);
                }
            }
        }

    }

    private NativeAffectedByForceArray BuildNativeAffectedArray(List<AffectedByForces> affectedByForcesList)
    {
        NativeAffectedByForceArray nativeAffectedArray = new NativeAffectedByForceArray
        {
            affectedPositions = new NativeArray<Vector3>(affectedByForcesList.Count, Allocator.TempJob),
            affectedForceStrengths = new NativeArray<float>(affectedByForcesList.Count, Allocator.TempJob)
        };

        for (int i = 0; i < affectedByForcesList.Count; i++)
        {
            nativeAffectedArray.affectedPositions[i] = affectedByForcesList[i].transform.position;
            nativeAffectedArray.affectedForceStrengths[i] = affectedByForcesList[i].ForceStrength;
        }

        return nativeAffectedArray;
    }

    public int GetCountOfAffectedByForce()
    {
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
