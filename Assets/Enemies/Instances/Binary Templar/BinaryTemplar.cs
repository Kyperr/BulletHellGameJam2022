using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target))]
public class BinaryTemplar : MonoBehaviour
{
    public enum Phase
    {
        CIRCLING,
        FOCUSSING
    }

    private const float DISTANCE_THRESHOLD = 1.0f;

    private GameObject target;

    [SerializeField]
    private float desiredDistance = 10f;

    [SerializeField]
    private float circlingAngle = 30f;

    [SerializeField]
    private Vector2 circlingTimeRange = new Vector2(3, 10);

    // When the AI is done circling, this is how long they pause to look at the player
    [SerializeField]
    private Vector2 focusOnPlayerTimeRange = new Vector2(5, 8);

    [SerializeField]
    private BulletPattern focusPhaseBulletPattern;

    [SerializeField]
    private float focusPhaseShotRate = 600;

    [SerializeField]
    private float moveSpeed = 10;

    [SerializeField]
    private float circlingSpeed = 30;

    private Phase phase = Phase.CIRCLING;

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
        phase = Phase.CIRCLING;
        timeToCircle = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
    }

    void Update()
    {
        if (target != null)
        {
            timeSpentOnPhase += Time.deltaTime;

            if (phase == Phase.CIRCLING)
            {
                CirclingLogic();
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

    private void CirclingLogic()
    {
        // The core of the logic
        UpdateDesiredAngle();
        MoveToDesiredPosition();

        // Is it time to swap phases?
        if (timeSpentOnPhase >= timeToCircle)
        {
            SwapToPhase(Phase.FOCUSSING);
            timeToCircle = Random.Range(focusOnPlayerTimeRange.x, focusOnPlayerTimeRange.y);
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
            SwapToPhase(Phase.CIRCLING);
            timeToCircle = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        }
    }

    private void SwapToPhase(Phase phase)
    {
        this.phase = phase;
        timeSpentOnPhase = 0;
        timeSinceLastShot = 0;
    }

    private void UpdateDesiredAngle()
    {
        desiredAngle += Time.deltaTime * circlingSpeed;
    }

    private void MoveToDesiredPosition()
    {
        Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * desiredAngle), 0, Mathf.Sin(Mathf.Deg2Rad * desiredAngle));
        Vector3 desiredPosition = this.target.transform.position + (directionalVector * desiredDistance);

        transform.LookAt(desiredPosition, Vector3.up);

        this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
    }

}
