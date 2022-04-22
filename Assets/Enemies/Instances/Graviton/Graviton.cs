using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Graviton : BaseEnemyAI
{

    private enum Phase
    {
        ABSORBING,
        ATTACKING,
        REPOSITIONING
    }

    [SerializeField]
    private float moveSpeed = 1;

    [SerializeField]
    private Vector2 distanceToMaintainFromPlayer = new Vector2(300, 400);

    [SerializeField]
    private EnemyBulletPattern attackingBulletPattern;

    [SerializeField]
    private float usualGravity = 0f;

    [SerializeField]
    private float absorbingGravity = 10000f;

    [SerializeField]
    private Vector2 timeToSpendAbsorbing = new Vector2(3, 8);

    [SerializeField]
    private GameObject suckingParticleSystem;

    [SerializeField]
    private VisualEffect absorbedVisualEffect;

    [SerializeField]
    private ParticleSystem shootParticleSystem;

    [SerializeField]
    private int minBulletsToShoot = 3;

    [SerializeField]
    private Vector2 rangeOfTimesToWaitBetweenShots = new Vector2(0.01f, 0.03f);

    private Vector3 desiredPosition;

    private GameObject target;

    private Phase phase;

    private float timeToSpendOnPhase = 0;

    private float timeSpentOnPhase = 0;

    private float timeSinceLastAttack = 0;

    private bool repositioning = false;

    private bool absorbing = false;

    private bool attacking = false;

    private int absorbedBullets = 0;

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
            timeSinceLastAttack += Time.deltaTime;

            if (phase == Phase.ATTACKING)
            {
                AttackLogic();
            }

            if (phase == Phase.REPOSITIONING)
            {
                RepositioningLogic();
            }

            if (phase == Phase.ABSORBING)
            {
                AbsorbingLogic();
            }
        }
    }

    private void AttackLogic()
    {
        Quaternion toRotation = Quaternion.LookRotation((target.transform.position - transform.position), transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5);

        // Fire a coroutine.
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {

        absorbedVisualEffect.gameObject.SetActive(false);

        int bulletsToShoot = Mathf.Max(absorbedBullets, minBulletsToShoot);
        absorbedBullets = 0;//reset now

        for (int i = 0; i < bulletsToShoot; i++)
        {
            StartCoroutine(attackingBulletPattern.TriggerBulletPattern(this.gameObject));
            shootParticleSystem.Play();
            yield return new WaitForSeconds(UnityEngine.Random.Range(rangeOfTimesToWaitBetweenShots.x, rangeOfTimesToWaitBetweenShots.y));

        }

        yield return new WaitForSeconds(2);

        SwapToPhase(Phase.REPOSITIONING);
        yield break;
    }

    private void RepositioningLogic()
    {
        if (!repositioning)
        {
            float distance = Random.Range(distanceToMaintainFromPlayer.x, distanceToMaintainFromPlayer.y);
            float newAngle = Random.Range(0, 360);
            Vector3 directionalVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), 0, Mathf.Sin(Mathf.Deg2Rad * newAngle));
            desiredPosition = this.target.transform.position + (directionalVector.normalized * distance);
            repositioning = true;
        }

        if (MoveToDesiredPosition(desiredPosition, moveSpeed))
        {
            repositioning = false;
            timeToSpendOnPhase = Random.Range(timeToSpendAbsorbing.x, timeToSpendAbsorbing.y);
            SwapToPhase(Phase.ABSORBING);
        }
    }

    private void AbsorbingLogic()
    {
        Quaternion toRotation = Quaternion.LookRotation((target.transform.position - transform.position), transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5);

        if (!absorbing)
        {
            absorbedVisualEffect.SetInt("StoredProjectiles", 0);
            absorbedVisualEffect.gameObject.SetActive(true);
            GetComponent<AffectingForce>().ForceStrength = absorbingGravity;
            GetComponent<TakesDamage>().TakeDamage = false;
            GetComponent<DestroysProjectileOnHit>().OnAny = true;
            GetComponent<DestroysProjectileOnHit>().OnProjectileDestroyed += OnDestroyProjectile;
            suckingParticleSystem.SetActive(true);
            absorbing = true;
        }


        absorbedVisualEffect.SetInt("StoredProjectiles", absorbedBullets);

        if (timeSpentOnPhase > timeToSpendOnPhase)
        {
            absorbing = false;
            GetComponent<DestroysProjectileOnHit>().OnAny = false;
            GetComponent<TakesDamage>().TakeDamage = true;
            GetComponent<DestroysProjectileOnHit>().OnProjectileDestroyed -= OnDestroyProjectile;
            GetComponent<AffectingForce>().ForceStrength = usualGravity;
            suckingParticleSystem.SetActive(false);
            SwapToPhase(Phase.ATTACKING);
        }
    }

    private void OnDestroyProjectile()
    {
        Debug.Log("Bullet absorbed.");
        absorbedBullets++;
    }

    private void SwapToPhase(Phase phase)
    {
        absorbing = false;
        this.phase = phase;
        timeSpentOnPhase = 0;
        attacking = false;
    }

}
