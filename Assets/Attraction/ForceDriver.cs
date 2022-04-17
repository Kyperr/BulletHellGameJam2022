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

        // The native list of jobhandles to complete together at the end.
        NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(affectingForcesList.Count, Allocator.TempJob);

        // These two match up to tell us which affectedByForce the force should apply to.
        List<int> affectedIndiciesList = new List<int>();
        List<NativeArray<Vector3>> forceVectorList = new List<NativeArray<Vector3>>();

        // Some intialization of variables.
        AffectingForce affectingForce;
        AffectedByForces sampledAffected;
        NativeArray<Vector3> forceVectors;
        int randomIndex;

        // Randomly choose the entities to sample and gather them into Native containers to be jobified.


        NativeAffectedByForceArray nativeAffectedArray = new NativeAffectedByForceArray
        {
            affectedPositions = new NativeArray<Vector3>(sampleCount, Allocator.TempJob),
            affectedForceStrengths = new NativeArray<float>(sampleCount, Allocator.TempJob)
        };

        for (int i = 0; i < sampleCount; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, numberOfAffected);
            affectedIndiciesList.Add(randomIndex);
            sampledAffected = affectedByForcesList[randomIndex];

            nativeAffectedArray.affectedPositions[i] = affectedByForcesList[randomIndex].transform.position;
            nativeAffectedArray.affectedForceStrengths[i] = affectedByForcesList[randomIndex].ForceStrength;
        }


        // Iterate over the affecting forces.
        for (int x = 0; x < affectingForcesList.Count; x++)
        {
            // The affecting force for this iteration
            affectingForce = affectingForcesList[x];

            // The native array to get populated
            forceVectors = new NativeArray<Vector3>(sampleCount, Allocator.TempJob);
            forceVectorList.Add(forceVectors);

            // Choose which ones to sample this frame. Add the random index to a list so we can use it as a reference later.
            List<AffectedByForces> sampledList = new List<AffectedByForces>(sampleCount);
            jobs[x] = affectingForce.CalculateForces(nativeAffectedArray, forceVectors);
        }

        // COMPLETE ALL THE JOBS
        JobHandle.CompleteAll(jobs);
        jobs.Dispose();
        nativeAffectedArray.affectedPositions.Dispose();
        nativeAffectedArray.affectedForceStrengths.Dispose();

        Vector3 vectorForce;
        Dictionary<int, Rigidbody> rigidBodyCache = new Dictionary<int, Rigidbody>();

        // Now apply all of the calculated forces.

        // For each affecting force, there should be a list of forces that correspond to an index of an entity.
        // for (int i = 0; i < affectingForcesList.Count; i++)
        // {
        //     // For each forceVector created by this affecting force
        //     for (int x = 0; x < forceVectorList[i].Length; x++)
        //     {
        //         Vector3 forceVector = forceVectorList[i][x];
        //         int affectedIndex = affectedIndiciesList[x];// This is the index that was calculated randomly earlier.
        //         sampledAffected = affectedByForcesList[affectedIndex];

        //         // Use a cache because GetComponent is a little expensive.
        //         if (rigidBodyCache.ContainsKey(affectedIndex))
        //         {
        //             rigidBodyCache[affectedIndex].AddForce(forceVector);
        //         }
        //         else
        //         {
        //             rigidBodyCache[affectedIndex] = sampledAffected.GetComponent<Rigidbody>();
        //             rigidBodyCache[affectedIndex].AddForce(forceVector);
        //         }
        //     }
        // }
        ///////////////
        // for each affected element
        // for each forceVector
        // there should be a force 
        Rigidbody rb;
        for (int i = 0; i < affectedIndiciesList.Count; i++)
        {
            int affectedIndex = affectedIndiciesList[i];// This is the index that was calculated randomly earlier.
            sampledAffected = affectedByForcesList[affectedIndex];

            // Get this patient's rigidbody
            // Use a cache because GetComponent is a little expensive.
            if (rigidBodyCache.ContainsKey(affectedIndex))
            {
                rb = rigidBodyCache[affectedIndex];
            }
            else
            {
                rb = sampledAffected.GetComponent<Rigidbody>();
                rigidBodyCache[affectedIndex] = rb;
            }

            // For each list of forces, there should be one at index i for this entity.

            for (int x = 0; x < forceVectorList.Count; x++)
            {
                Vector3 forceVector = forceVectorList[x][i];
                rb.AddForce(forceVector);
            }

        }
        ////////////////
        // Now clean up this massive mess
        foreach (NativeArray<Vector3> nativeContainer in forceVectorList)
        {
            nativeContainer.Dispose();
        }

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
