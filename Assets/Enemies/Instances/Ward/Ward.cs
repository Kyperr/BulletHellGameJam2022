using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : BaseEnemyAI
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
    private float blackHoleDeletionRate = 10;

    [SerializeField]
    private EnemyBulletPattern attackingBulletPattern;

    [SerializeField]
    private float fleeTriggerDistance = 10;

    [SerializeField]
    private float blackholeDeletingAnimationTime = 3f;

    [SerializeField]
    private LineRenderer blackholeDeletingLineRenderer;

    private Vector3 desiredPosition;

    private GameObject target;

    private Phase phase;

    private float timeToSpendOnPhase = 0;

    private float timeSpentOnPhase = 0;

    private float timeSinceLastAttack = 0;

    private float timeSinceBlackHoleDeletion = 0;

    private bool repositioning = false;

    private bool attacking = false;

    void Start()
    {
        target = EnemySpawner.Instance.Target;
        this.GetComponent<Target>().SetTarget(EnemySpawner.Instance.Target);
        phase = Phase.REPOSITIONING;
        timeSpentOnPhase = 0;
        timeToSpendOnPhase = -1;
        timeSinceLastAttack = float.MaxValue;
        timeSinceBlackHoleDeletion = float.MaxValue;
    }

    void Update()
    {
        if (target != null)
        {
            timeSpentOnPhase += Time.deltaTime;
            timeSinceLastAttack += Time.deltaTime;

            // Done all the time
            DeleteBlackHoleLogic();

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
        Quaternion toRotation = Quaternion.LookRotation((target.transform.position - transform.position), transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5);

        if (timeSinceLastAttack > (60f / (float)bulletShotRate))
        {
            StartCoroutine(ChargeAndAttack());
            timeSinceLastAttack = 0;
        }

        if (timeSpentOnPhase > timeToSpendOnPhase && !attacking)
        {
            SwapToPhase(Phase.REPOSITIONING);
        }
    }

    private IEnumerator ChargeAndAttack()
    {

        attacking = true;

        StartCoroutine(attackingBulletPattern.TriggerBulletPattern(this.gameObject));

        attacking = false;

        yield break;
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

    private void DeleteBlackHoleLogic()
    {
        timeSinceBlackHoleDeletion += Time.deltaTime;
        if (timeSinceBlackHoleDeletion >= (60f / (float)blackHoleDeletionRate))
        {
            Debug.Log("Deleting a black hole.");
            foreach (AffectingForce affectingForce in ForceDriver.Instance.AffectingForcesList)
            {
                BlackHole blackHole = affectingForce.GetComponent<BlackHole>();
                if (blackHole)
                {
                    // Delete
                    StartCoroutine(DeleteBlackHole(blackHole));
                    timeSinceBlackHoleDeletion = 0;
                }
            }
        }
    }

    private IEnumerator DeleteBlackHole(BlackHole blackHole)
    {
        blackholeDeletingLineRenderer.gameObject.SetActive(true);
        float animationTime = 0;
        while (animationTime < blackholeDeletingAnimationTime)
        {
            blackholeDeletingLineRenderer.SetPosition(0, transform.position);
            blackholeDeletingLineRenderer.SetPosition(1, blackHole.transform.position);
            animationTime += Time.deltaTime;
            yield return 0;
        }
        Debug.Log("Did animation, deleting black hole now..");
        blackholeDeletingLineRenderer.gameObject.SetActive(false);
        PopupManager.Instance.createPopupText(blackHole.transform.position, "DESTROYED", Color.red);
        Destroy(blackHole.gameObject);

        yield break;
    }

    private void SwapToPhase(Phase phase)
    {
        repositioning = false;
        this.phase = phase;
        timeSpentOnPhase = 0;
    }

}
