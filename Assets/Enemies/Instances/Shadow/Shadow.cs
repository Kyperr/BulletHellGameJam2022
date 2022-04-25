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
    private GameObject shadowClone;

    [SerializeField]
    private int dashCount = 4;

    [SerializeField]
    private float dashPauseTime = .5f;

    [SerializeField]
    private float dashSpeed = 250f;

    [SerializeField]
    private float dashAngle = 80f;

    [SerializeField]
    private float dashDistance = 50f;

    private Vector2 shadowStart;

    private Phase phase;

    private float timeSinceLastShot = 0;
    private bool distancing = false;
    private bool dashing = false;
    private Vector3 desiredPosition;

    private float timeToSpendOnPhase = 0;

    private float timeSpentOnPhase = 0;

    void Start()
    {
        target = EnemySpawner.Instance.Target;
        this.GetComponent<Target>().SetTarget(EnemySpawner.Instance.Target);
        this.phase = Phase.DISTANCING;
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

    private void DashingLogic()
    {
        if (!dashing)
        {
            dashing = true;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Vector3 direction = (target.transform.position - this.transform.position).normalized;
        Vector3[] dashPoints = new Vector3[dashCount];
        float[] bulletDirections = new float[dashCount];
        Debug.Log("dashPoints : " + dashPoints.Length);
        Vector3 dashAngleVector; //= new Vector3(Mathf.Sin(Mathf.Deg2Rad * dashAngle), 0, Mathf.Cos(Mathf.Deg2Rad * dashAngle));

        float sign = Mathf.Sign(Random.Range(-1f, 1f));
        bulletDirections[0] = sign;
        Vector3 lastPoint = transform.position;

        for (int i = 0; i < dashCount; i++)
        {
            // dashAngleVector = new Vector3(Mathf.Sin(Mathf.Deg2Rad * dashAngle * sign), 0, Mathf.Cos(Mathf.Deg2Rad * dashAngle * sign)).normalized;
            dashAngleVector = Quaternion.AngleAxis(dashAngle * sign, Vector3.up) * direction;
            if (i == 0 || i == dashCount - 1)
            {
                // Half distance on the first and last to maintain center.
                dashPoints[i] = lastPoint + (direction + dashAngleVector).normalized * (dashDistance / 2);
            }
            else
            {
                dashPoints[i] = lastPoint + (direction + dashAngleVector).normalized * dashDistance;
            }

            lastPoint = dashPoints[i];
            bulletDirections[i] = sign;
            sign *= -1;
        }

        for (int i = 0; i < dashPoints.Length; i++)
        {
            Vector3 point = dashPoints[i];
            float bullletDirection = bulletDirections[i];

            // Do the moving
            while (!MoveToDesiredPosition(point, dashSpeed, false))
            {
                yield return 0;
            }

            // Send the bullets
            if (bullletDirection == 1)
            {
                StartCoroutine(attackPattern.TriggerBulletPattern(this.gameObject, null, transform.right));
            }

            if (bullletDirection == -1)
            {
                StartCoroutine(attackPattern.TriggerBulletPattern(this.gameObject, null, transform.right * -1));
            }

            yield return new WaitForSeconds(dashPauseTime);
        }

        dashing = false;

        SwapToPhase(Phase.DISTANCING);
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
            StartCoroutine(MoveToDistance());
        }

    }

    private IEnumerator MoveToDistance()
    {
        while (!MoveToDesiredPosition(desiredPosition, moveSpeed))
        {
            yield return 0;
        }

        float speed = 1;

        float timeLerping = 0;

        // Look at player;
        Quaternion toRotation = Quaternion.LookRotation((target.transform.position - transform.position), transform.up);
        while (timeLerping < speed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, timeLerping / speed);
            timeLerping += Time.deltaTime;
            yield return 0;
        }

        distancing = false;
        SwapToPhase(Phase.DASHING);
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
