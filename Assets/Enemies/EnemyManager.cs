using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    Dictionary<string, bool> unlockedEnemy = new Dictionary<string, bool>();

    public void unlockEnemy(string n)
    {
        unlockedEnemy[n] = true;
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
