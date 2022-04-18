using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ExplosionSpawner))]
public class HPObject : MonoBehaviour
{
    [SerializeField]
    float maxHP;
    [SerializeField]
    float currentHP;
    bool isDead;
    HPBar hpBar;
    ExplosionSpawner explosion;
    EnemySpawner enemySpawner;

    public void DoDamage(float d)
    {
        if (isDead)
        {
            return;
        }
        currentHP -= d;
        Mathf.Clamp(currentHP, 0, maxHP);
        if (currentHP <= 0)
        {
            Die();
        }

        if (hpBar)
        {
            //update HPBar
            hpBar.update(currentHP);
        }
    }
    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        //explosion
        explosion.spawn();
        DisableMesheRenderers();
        Destroy(gameObject);
        enemySpawner.destroy(GetComponentInParent<SpawnableEnemy>());
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
        if (!hpBar)
        {
            Debug.LogError("hp bar component does not existed as a child");
        }
        else
        {
            hpBar.init(maxHP);
        }
        explosion = GetComponent<ExplosionSpawner>();
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
