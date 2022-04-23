using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;

    public static EnemySpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<EnemySpawner>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject target;
    public GameObject Target => target;

    [SerializeField]
    EnemySpawnLogic spawnLogic;

    [SerializeField]
    private FMODUnity.StudioEventEmitter musicObject;

    int currentRound = 0;
    public int CurrentRound { get { return currentRound; } }
    private List<SpawnableEnemy> enemyList = new List<SpawnableEnemy>();
    public List<SpawnableEnemy> EnemyList => enemyList;

    Arena arena;
    SimpleControls player;
    public Vector3 PlayerPosition { get { return player.transform.position; } }
    // Start is called before the first frame update
    void Start()
    {
        arena = GameObject.FindObjectOfType<Arena>();
        player = GameObject.FindObjectOfType<SimpleControls>();
        generateNextRound();
    }
    public float ArenaRadius { get { return arena.ArenaRadius; } }

    //just a test function for simulate multiple rounds while enemy can't be killed now
    public void clearEnemies()
    {
        foreach (var enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }

    public void destroy(SpawnableEnemy enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            if (enemyList.Count == 0)
            {
                generateNextRound();
            }
        }
        else
        {
            Debug.LogError("enemy is not in the list");
        }
    }

    [ContextMenu("generate next round")]
    public void generateNextRound()
    {
        //might not need this, this is for testing
        clearEnemies();
        spawnLogic.generateEnemies(this);
        currentRound++;
        musicObject.SetParameter("Waves", currentRound);
    }

    public void addEnemy(SpawnableEnemy enemy)
    {
        enemyList.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            generateNextRound();
        }
    }
}
