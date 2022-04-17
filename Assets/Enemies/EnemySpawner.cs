using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemySpawnLogic spawnLogic;
    int currentRound = 0;
    public int CurrentRound { get { return currentRound; } }
    List<SpawnableEnemy> enemyList = new List<SpawnableEnemy>();
    // Start is called before the first frame update
    void Start()
    {

    }

    //just a test function for simulate multiple rounds while enemy can't be killed now
    public void clearEnemies()
    {
        foreach (var enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }

    [ContextMenu("generate next round")]
    public void generateNextRound()
    {
        //might not need this, this is for testing
        clearEnemies();
        spawnLogic.generateEnemies(this);
        currentRound++;
    }

    public void addEnemy(SpawnableEnemy enemy)
    {
        enemyList.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
