using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnLogic", menuName = "ScriptableObjects/EnemySpawnLogic/EnemySpawnLogic", order = 1)]
public class EnemySpawnLogic : ScriptableObject
{
    [SerializeField]
    List<SpawnableEnemy> enemyList;
    //[SerializeField]
    //List<int> roundBudgets;
    [SerializeField]
    int startBudget;
    [SerializeField]
    int increaseBudgetPerRound;
    [SerializeField]
    int maxBudget;
    [SerializeField]
    float enemyRadius = 2f;


    float calcualteMinBudget()
    {

        float minimalBudget = float.MaxValue;
        foreach (var enemy in enemyList)
        {
            minimalBudget = Mathf.Min(minimalBudget, enemy.SpawnCost);
        }
        return minimalBudget;
    }

    Vector3 validGenerationPosition(List<Vector3> avoidCollisionList,float radius)
    {
        int loopCount = 0;
        while (loopCount < 100)
        {
            Vector2 position2d = Random.insideUnitCircle * radius;
            Vector3 position = new Vector3(position2d.x, 0, position2d.y);
            bool isValid = true;
            foreach(var p in avoidCollisionList)
            {
                if((p-position).sqrMagnitude< enemyRadius)
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                return position;
            }
                loopCount++;
        }
        return Vector3.positiveInfinity;
    }

    public void generateEnemies(EnemySpawner es)
    {
        float radius = es.ArenaRadius;
        var minimalBudget = calcualteMinBudget();
        float budget = startBudget + es.CurrentRound * increaseBudgetPerRound;
        budget = Mathf.Min(budget, maxBudget);
        //Debug.Log("current budge " + budget);
        int loopCount = 0;//in case for any reason we get into infinite loop

        List<Vector3> avoidCollisionList = new List<Vector3>() { es.PlayerPosition };
        List<GameObject> enemies = new List<GameObject>();
        while (loopCount < 100 && budget >= minimalBudget)
        {
            var enemy = enemyList[Random.Range(0, enemyList.Count)];
            if (budget - enemy.SpawnCost >= 0)
            {
                var position = validGenerationPosition(avoidCollisionList, radius);
                if(position == Vector3.positiveInfinity)
                {
                    Debug.LogError("can't find a good position for enemy");
                    return;
                }
                if(!EnemyManager.Instance.isEnemyUnlocked( enemy.name))
                {
                    //if enemy not unlocked. remove others and stop here.
                    es.clearEnemies();
                }
                var go = Instantiate(enemy.gameObject, position, Quaternion.identity, es.transform);
                es.addEnemy(go.GetComponent<SpawnableEnemy>());
                avoidCollisionList.Add(position);
                budget -= enemy.SpawnCost;
                if (!EnemyManager.Instance.isEnemyUnlocked(enemy.name))
                {
                    EnemyManager.Instance.unlockEnemy(enemy.name);
                    break;
                }

            }
            loopCount++;
        }
    }
}
