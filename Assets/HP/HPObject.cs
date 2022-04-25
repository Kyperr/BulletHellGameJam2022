using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath = delegate { };

    [SerializeField]
    float maxHP;
    public float MaxHP => maxHP;

    [SerializeField]
    float currentHP;
    public float CurrentHP => currentHP;

    bool isDead;

    HPBar hpBar;

    EnemySpawner enemySpawner;

    public bool DoDamage(float d)
    {
        if (isDead)
        {
            return false;
        }
        currentHP -= d;
        Mathf.Clamp(currentHP, 0, maxHP);

        if (hpBar)
        {
            //update HPBar
            hpBar.update(currentHP);
        }

        if (currentHP <= 0)
        {
            Die();
            return true;
        }
        else
        {
            return false;
        }

    }

    public void DoHealing(float d)
    {
        currentHP = Mathf.Min(maxHP, currentHP + d);
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        OnDeath();
        DisableMesheRenderers();
        if (hpBar)
        {
            Destroy(hpBar.gameObject);
        }
        Destroy(gameObject);

        // IF its an enemy, update the enemy spawner.
        if (GetComponent<SpawnableEnemy>())
        {
            //if not player
            enemySpawner.destroy(GetComponentInParent<SpawnableEnemy>());
        }

        // If its a player, oopsies.
        if (GetComponent<SimpleControls>())
        {
            //game over
            GameOverView goView = GameObject.FindObjectOfType<GameOverView>(true);
            goView.onGameOver();
        }
    }


    private void DisableMesheRenderers()
    {
        foreach (Renderer r in this.GetComponentsInChildren(typeof(Renderer)))
        {
            r.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (maxHP <= 0)
        {
            Debug.LogError("max hp should not be 0");
        }
        currentHP = maxHP;
        hpBar = GetComponentInChildren<HPBar>();
        if (hpBar)
        {
            hpBar.init(maxHP, this);
        }
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //for test only
        if (Input.GetKeyDown(KeyCode.P))
        {
            //getDamage(50);
            DoDamage(Random.Range(30, 100));
        }
    }
}
