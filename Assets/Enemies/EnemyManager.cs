using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    int currentEnemyIndex = 0;
    Dictionary<string, bool> unlockedEnemy = new Dictionary<string, bool>();

    public void unlockEnemy(string n)
    {
        unlockedEnemy[n] = true;
        currentEnemyIndex++;
        Debug.Log("curent enemy index increase to " + currentEnemyIndex);
    }

    public SpawnableEnemy nextUnlockEnemy(List<SpawnableEnemy> enemyList)
    {
        if (currentEnemyIndex >= enemyList.Count)
        {
            return null;
        }
        return enemyList[currentEnemyIndex];
    }
    public bool isEnemyUnlocked(string n)
    {
        return unlockedEnemy.ContainsKey(n);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
