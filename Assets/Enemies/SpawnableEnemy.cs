using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableEnemy : MonoBehaviour
{


    [SerializeField]
    private float spawnCost = 3f;
    EnemySpawner enemySpawner;

    public float SpawnCost { get { return spawnCost; } }

    public void init(EnemySpawner es)
    {
        enemySpawner = es;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
