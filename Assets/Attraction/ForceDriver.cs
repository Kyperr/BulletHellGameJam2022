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

    [SerializeField]
    private float forceIntensityModifier = 1.0f;

    [SerializeField]
    private int numberOfAffected = 0;

    [SerializeField]
    private int numberOfAffecting = 0;

    private List<AffectedByForces> affectedByForcesList = new List<AffectedByForces>();

    private List<AffectingForce> affectingForcesList = new List<AffectingForce>();
    public List<AffectingForce> AffectingForcesList => affectingForcesList;

    // To start, this is going to simply approximate gravity.
    void Update()
    {
        if (affectedByForcesList.Count == 0)
        {
            return;
        }

        numberOfAffected = affectedByForcesList.Count;
        numberOfAffecting = affectingForcesList.Count;

        // The native list of jobhandles to complete together at the end.
        NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(affectingForcesList.Count, Allocator.TempJob);

        // These two match up to tell us which affectedByForce the force should apply to.
        List<NativeArray<Vector3>> forceVectorList = new List<NativeArray<Vector3>>(affectingForcesList.Count);

        // Some intialization of variables.
        AffectingForce affectingForce;
        NativeArray<Vector3> forceVectors;

        // Build the native affectedArray
        NativeAffectedByForceArray nativeAffectedByForceArray = BuildNativeAffectedArray(affectedByForcesList);

        // Iterate over the affecting forces.
        for (int x = 0; x < affectingForcesList.Count; x++)
        {
            // The affecting force for this iteration
            affectingForce = affectingForcesList[x];

            // The native array to get populated
            forceVectors = new NativeArray<Vector3>(affectedByForcesList.Count, Allocator.TempJob);
            forceVectorList.Add(forceVectors);

            // Choose which ones to sample this frame. Add the random index to a list so we can use it as a reference later.
            List<AffectedByForces> sampledList = new List<AffectedByForces>(affectingForcesList.Count);
            jobs[x] = affectingForce.CalculateForces(nativeAffectedByForceArray, forceVectors);
        }

        // COMPLETE ALL THE JOBS
        JobHandle.CompleteAll(jobs);
        jobs.Dispose();
        nativeAffectedByForceArray.affectedPositions.Dispose();
        nativeAffectedByForceArray.affectedForceStrengths.Dispose();

        // Now apply all of the calculated forces.
        ApplyCalculatedForces(forceVectorList);


        // Now clean up this massive mess
        foreach (NativeArray<Vector3> nativeContainer in forceVectorList)
        {
            nativeContainer.Dispose();
        }

    }

    private void ApplyCalculatedForces(List<NativeArray<Vector3>> nativeForceVectorList)
    {

        List<Vector3[]> forceVectorList = new List<Vector3[]>();
        foreach (NativeArray<Vector3> forceArray in nativeForceVectorList)
        {
            Vector3[] list = new Vector3[forceArray.Length];
            NativeArray<Vector3>.Copy(forceArray, list, forceArray.Length);
            forceVectorList.Add(list);
        }


        // A cache so we are N^2'ing GetComponents
        Dictionary<int, Rigidbody> rigidBodyCache = new Dictionary<int, Rigidbody>();

        //Init some vars
        int forceVectorListCount = forceVectorList.Count;
        AffectedByForces affected;
        Vector3 vectorForce;

        // For each affected force, there will be a forceVector in the forceVectorList
        for (int i = 0; i < affectedByForcesList.Count; i++)
        {
            // Get the rigid body
            Rigidbody rb;
            affected = affectedByForcesList[i];
            if (!rigidBodyCache.TryGetValue(i, out rb))
            {
                rb = affected.GetComponent<Rigidbody>();
                rigidBodyCache[i] = rb;
            }

            if (rb)
            {
                // Add all of the vector forces together for this RB
                vectorForce = new Vector3();

                // Add all forceVectors to this rigidbody.
                for (int x = 0; x < forceVectorListCount; x++)
                {
                    vectorForce += forceVectorList[x][i] * forceIntensityModifier;
                }

                rb.AddForce(vectorForce);
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
