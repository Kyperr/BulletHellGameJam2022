using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : BaseEnemyAI
{
    private const float DESIRED_POSITION_THRESHOLD = 5.0f;
    public enum Phase
    {
        CHASING,
        DISTANCING,
        DASHING
    }

    private GameObject target;

    [SerializeField]
    private float distanceToChaseTo = 100f;

    [SerializeField]
    private Vector2 rangeOfDistancing = new Vector2(20, 60);

    [SerializeField]
    private EnemyBulletPattern attackPattern;

    [SerializeField]
    private float rateOfFire = 300;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float distanceToAvoidOtherEnemies = 30f;

    [SerializeField]
    private List<GameObject> shadowClones;

    private Phase phase;

    private float timeSinceLastShot = 0;
    private bool distancing = false;
    private Vector3 desiredPosition;

    private float timeToSpendOnPhase = 0;

    private float timeSpentOnPhase = 0;

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

            MoveShadowClones();

            if (phase == Phase.CHASING)
            {
                ChasingLogic();
            }

            if (phase == Phase.DISTANCING)
            {
                DistancingLogic();
            }

            if (phase == Phase.DASHING)
            {
                DashingLogic();
            }
        }
    }

    private void MoveShadowClones()
    {
        foreach (GameObject shadow in shadowClones)
        {
            float xPos = transform.position.x + Mathf.PingPong(Time.time, 5);
            float zPos = transform.position.z + Mathf.PingPong(Time.time, 5);

            shadow.transform.position = new Vector3(xPos, 0, zPos);
        }
    }

    private void DashingLogic()
    {
        SwapToPhase(Phase.CHASING);
    }

    private void DistancingLogic()
    {
        if (!distancing)
        {
            float distance = Random.Range(rangeOfDistancing.x, rangeOfDistancing.y);
            float newAngle = Random.Range(0, 360);
            Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), 0, Mathf.Sin(Mathf.Deg2Rad * newAngle));
            desiredPosition = this.target.transform.position + (directionalVector.normalized * distance);
            distancing = true;
        }

        if (MoveToDesiredPosition(desiredPosition, moveSpeed))
        {
            distancing = false;
            SwapToPhase(Phase.DASHING);
        }
    }

    private void ChasingLogic()
    {
        if (Vector3.Distance(target.transform.position, transform.position) > distanceToChaseTo)
        {
            Vector3 desiredPosition = target.transform.position + ((target.transform.position - transform.position).normalized * distanceToChaseTo);

            MoveToDesiredPosition(desiredPosition, moveSpeed);
            ConsiderOtherEnemies();

        }
        else
        {
            SwapToPhase(Phase.DISTANCING);
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
