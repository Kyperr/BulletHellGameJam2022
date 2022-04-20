using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoplite : BaseEnemyAI
{

    private enum Phase
    {
        ATTACKING,
        REPOSITIONING,
        FLEEING
    }

    [SerializeField]
    private float moveSpeed = 1;

    [SerializeField]
    private Vector2 rangeOfAttackingDistances = new Vector2(20, 60);

    [SerializeField]
    private Vector2 timeToSpendAttacking = new Vector2(6, 10);

    [SerializeField]
    private float bulletShotRate = 20;

    [SerializeField]
    private BulletPattern attackingBulletPattern;


    [SerializeField]
    private float fleeTriggerDistance = 10;

    private Vector3 desiredPosition;

    private GameObject target;

    private Phase phase;

    private float timeToSpendOnPhase = 0;

    private float timeSpentOnPhase = 0;

    private float timeSinceLastAttack = 0;

    private bool repositioning = false;

    void Start()
    {
        target = EnemySpawner.Instance.Target;
        this.GetComponent<Target>().SetTarget(EnemySpawner.Instance.Target);
        phase = Phase.REPOSITIONING;
        timeSpentOnPhase = 0;
        timeToSpendOnPhase = -1;
    }

    void Update()
    {
        if (target != null)
        {
            timeSpentOnPhase += Time.deltaTime;
            CheckIfShouldFlee();

            timeSpentOnPhase += Time.deltaTime;

            if (phase == Phase.ATTACKING)
            {
                AttackLogic();
            }

            if (phase == Phase.REPOSITIONING)
            {
                RepositioningLogic();
            }

            if (phase == Phase.FLEEING)
            {
                FleeLogic();
            }
        }
    }

    private void CheckIfShouldFlee()
    {
        if (Vector3.Distance(target.transform.position, this.transform.position) < fleeTriggerDistance)
        {
            SwapToPhase(Phase.FLEEING);
        }
    }

    private void AttackLogic()
    {
        transform.LookAt(target.transform.position, Vector3.up);

        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > (60f / (float)bulletShotRate))
        {
            // TODO charge up animation
            StartCoroutine(attackingBulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastAttack = 0;
        }

        if (timeSpentOnPhase > timeToSpendOnPhase)
        {
            SwapToPhase(Phase.REPOSITIONING);
        }
    }

    private void FleeLogic()
    {
        // TODO Dissapear flash.

        float distance = Random.Range(rangeOfAttackingDistances.x, rangeOfAttackingDistances.y);
        float newAngle = Random.Range(0, 360);
        Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), 0, Mathf.Sin(Mathf.Deg2Rad * newAngle));
        Vector3 desiredPosition = this.target.transform.position + (directionalVector.normalized * distance);
        transform.position = desiredPosition;


        // TODO Reappear flash and become visible;


        timeToSpendOnPhase = Random.Range(timeToSpendAttacking.x, timeToSpendAttacking.y);
        SwapToPhase(Phase.ATTACKING);
    }

    private void RepositioningLogic()
    {
        if (!repositioning)
        {
            float distance = Random.Range(rangeOfAttackingDistances.x, rangeOfAttackingDistances.y);
            float newAngle = Random.Range(0, 360);
            Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), 0, Mathf.Sin(Mathf.Deg2Rad * newAngle));
            desiredPosition = this.target.transform.position + (directionalVector.normalized * distance);
            repositioning = true;
        }

        if (MoveToDesiredPosition(desiredPosition, moveSpeed))
        {
            repositioning = false;
            timeToSpendOnPhase = Random.Range(timeToSpendAttacking.x, timeToSpendAttacking.y);
            SwapToPhase(Phase.ATTACKING);
        }
    }

    private void SwapToPhase(Phase phase)
    {
        repositioning = false;
        this.phase = phase;
        timeSpentOnPhase = 0;
    }

}
