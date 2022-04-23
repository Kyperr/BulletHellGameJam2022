using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : BaseEnemyAI
{
    private const float DESIRED_POSITION_THRESHOLD = 5.0f;
    public enum Phase
    {
        CHASING,
        ATTACKING
    }

    private GameObject target;

    [SerializeField]
    private float desiredDistanceFromPlayer = 50f;

    [SerializeField]
    private float distanceToAttackFrom = 60f;

    [SerializeField]
    private EnemyBulletPattern attackPattern;

    [SerializeField]
    private float rateOfFire = 300;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float distanceToAvoidOtherEnemies = 30f;

    private Phase phase;

    private float timeSinceLastShot = 0;


    void Start()
    {
        target = EnemySpawner.Instance.Target;
        this.GetComponent<Target>().SetTarget(EnemySpawner.Instance.Target);
        this.phase = Phase.CHASING;
    }

    void Update()
    {
        if (target != null)
        {
            timeSinceLastShot += Time.deltaTime;

            if (phase == Phase.CHASING)
            {
                ChasingLogic();
            }
        }
    }

    private void ChasingLogic()
    {

        if (Vector3.Distance(target.transform.position, transform.position) <= distanceToAttackFrom)
        {
            TryAttack();
        }
        else
        {

            Vector3 desiredPosition = target.transform.position + ((target.transform.position - transform.position).normalized * desiredDistanceFromPlayer);

            MoveToDesiredPosition(desiredPosition, moveSpeed);
            ConsiderOtherEnemies();

        }
    }

    private void ConsiderOtherEnemies()
    {
        foreach (SpawnableEnemy enemy in EnemySpawner.Instance.EnemyList)
        {
            if (enemy.gameObject != this.gameObject && Vector3.Distance(enemy.transform.position, this.transform.position) <= distanceToAvoidOtherEnemies)
            {
                Vector3 desiredPosition = (this.transform.position - enemy.transform.position).normalized * distanceToAvoidOtherEnemies;
                MoveToDesiredPosition(desiredPosition, moveSpeed);
            }
        }
    }

    private void TryAttack()
    {
        if (timeSinceLastShot > (60f / (float)rateOfFire))
        {
            StartCoroutine(attackPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastShot = 0;
        }
    }

    private void SwapToPhase(Phase phase)
    {
        this.phase = phase;
    }

}
