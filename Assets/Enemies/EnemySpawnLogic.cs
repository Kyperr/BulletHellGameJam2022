using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnLogic", menuName = "ScriptableObjects/EnemySpawnLogic/EnemySpawnLogic", order = 1)]
public class EnemySpawnLogic : ScriptableObject
{
    [SerializeField]
    List<SpawnableEnemy> enemyList;
    [SerializeField]
    List<int> roundBudgets;

    float radius = 10;//todo read from arena

    float calcualteMinBudget()
    {

        float minimalBudget = float.MaxValue;
        foreach (var enemy in enemyList)
        {
            minimalBudget = Mathf.Min(minimalBudget, enemy.SpawnCost);
        }
        return minimalBudget;
    }

    public void generateEnemies(EnemySpawner es)
    {
        var minimalBudget = calcualteMinBudget();
        float budge = roundBudgets[Mathf.Min(es.CurrentRound, roundBudgets.Count - 1)];
        int loopCount = 0;//in case for any reason we get into infinite loop
        while (loopCount < 1000 && budge >= minimalBudget)
        {
            var enemy = enemyList[Random.Range(0, enemyList.Count)];
            if (budge - enemy.SpawnCost >= 0)
            {
                //todo make sure enemies wont collide each other
                Vector3 position = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
                var go = Instantiate(enemy.gameObject, position, Quaternion.identity, es.transform);
                es.addEnemy(go.GetComponent<SpawnableEnemy>());
                budge -= enemy.SpawnCost;
            }
            loopCount++;
        }
    }
}
