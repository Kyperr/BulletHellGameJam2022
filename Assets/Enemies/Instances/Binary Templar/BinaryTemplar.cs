using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTemplar : BaseEnemyAI
{
    private const float DESIRED_POSITION_THRESHOLD = 5.0f;
    public enum Phase
    {
        CIRCLING_CLOCKWISE,
        CIRCLING_COUNTER_CLOCKWISE,
        DISTANCING,
        FOCUSSING
    }

    private const float DISTANCE_THRESHOLD = 1.0f;

    private GameObject target;

    [SerializeField]
    private float circlingDistance = 10f;

    [SerializeField]
    private float circlingAngle = 30f;

    [SerializeField]
    private Vector2 circlingTimeRange = new Vector2(3, 10);

    // When the AI is done circling, this is how long they pause to look at the player
    [SerializeField]
    private Vector2 focusOnPlayerTimeRange = new Vector2(5, 8);


    [SerializeField]
    private float focussingDistance = 25f;

    [SerializeField]
    private EnemyBulletPattern focusPhaseBulletPattern;

    [SerializeField]
    private EnemyBulletPattern circlingPhaseBulletPattern;

    [SerializeField]
    private float focusPhaseShotRate = 10;

    [SerializeField]
    private float circlingPhaseShotRate = 30;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float circlingAngleDelta = 5;

    private Phase phase;

    private float desiredAngle = 0;

    private float timeSpentOnPhase = 0;
    private float timeSinceLastShot = 0;

    private float timeToCircle = 0;


    void Start()
    {
        target = EnemySpawner.Instance.Target;
        this.GetComponent<Target>().SetTarget(EnemySpawner.Instance.Target);
        desiredAngle = Random.Range(0, 360);
        timeSpentOnPhase = 0;
        this.phase = ChooseRandomCirclingLogic();
        timeToCircle = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
    }

    void Update()
    {
        if (target != null)
        {
            timeSpentOnPhase += Time.deltaTime;
            timeSinceLastShot += Time.deltaTime;

            if (phase == Phase.CIRCLING_CLOCKWISE)
            {
                CWCirclingLogic();
            }

            if (phase == Phase.CIRCLING_COUNTER_CLOCKWISE)
            {
                CCWCirclingLogic();
            }

            if (phase == Phase.DISTANCING)
            {
                DistancingLogic();
            }

            if (phase == Phase.FOCUSSING)
            {
                FocussingLogic();
            }

            // OLD STUFF: Just for reference.
            // 1) Enemy should circle the player for N seconds
            // 2) After N seconds, stop, turn and face player.
            // 3) Shoot bullets towards player and wait for X amount of seconds (configurable)
            // 4) Continue circling the player, randomly choose clockwise or counter clockwise.
        }
    }

    private Phase ChooseRandomCirclingLogic()
    {
        if (Random.Range(0, 1) == 0)
        {
            return Phase.CIRCLING_COUNTER_CLOCKWISE;
        }
        else
        {
            return Phase.CIRCLING_CLOCKWISE;
        }
    }

    private void CWCirclingLogic()
    {
        // The core of the logic
        UpdateDesiredAngle();
        MoveToDesiredPosition(GetNextPositionInCirclingPlayer(), moveSpeed);

        TryCirclingAttack();

        // Is it time to swap phases?
        if (timeSpentOnPhase >= timeToCircle)
        {
            SwapToPhase(Phase.DISTANCING);
            timeToCircle = Random.Range(focusOnPlayerTimeRange.x, focusOnPlayerTimeRange.y);
        }
    }

    private void CCWCirclingLogic()
    {
        // The core of the logic
        UpdateDesiredAngle();
        MoveToDesiredPosition(GetNextPositionInCirclingPlayer(), moveSpeed);

        TryCirclingAttack();

        // Is it time to swap phases?
        if (timeSpentOnPhase >= timeToCircle)
        {
            SwapToPhase(Phase.DISTANCING);
            timeToCircle = Random.Range(focusOnPlayerTimeRange.x, focusOnPlayerTimeRange.y);
        }
    }

    private void TryCirclingAttack()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > (60f / (float) circlingPhaseShotRate))
        {
            StartCoroutine(circlingPhaseBulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastShot = 0;
        }
    }

    private void DistancingLogic()
    {
        Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * desiredAngle), 0, Mathf.Sin(Mathf.Deg2Rad * desiredAngle));
        Vector3 desiredPosition = this.target.transform.position + (directionalVector * focussingDistance);
        if (MoveToDesiredPosition(desiredPosition, moveSpeed))
        {
            SwapToPhase(Phase.FOCUSSING);
        }
    }

    private void FocussingLogic()
    {

        transform.LookAt(target.transform.position, Vector3.up);

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > (60f / (float)focusPhaseShotRate))
        {
            StartCoroutine(focusPhaseBulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastShot = 0;
        }

        // Is it time to swap phases?
        if (timeSpentOnPhase >= timeToCircle)
        {
            SwapToPhase(ChooseRandomCirclingLogic());
            timeToCircle = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        }
    }

    private void SwapToPhase(Phase phase)
    {
        this.phase = phase;
        timeSpentOnPhase = 0;
        timeSinceLastShot = 0;
    }

    private void UpdateDesiredAngle(bool clockWise = true)
    {
        if (clockWise)
        {
            desiredAngle += Time.deltaTime * circlingAngleDelta;
        }
        else
        {
            desiredAngle -= Time.deltaTime * circlingAngleDelta;
        }
    }

    private Vector3 GetNextPositionInCirclingPlayer()
    {
        Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * desiredAngle), 0, Mathf.Sin(Mathf.Deg2Rad * desiredAngle));
        Vector3 desiredPosition = this.target.transform.position + (directionalVector.normalized * circlingDistance);
        return desiredPosition;
    }

}
