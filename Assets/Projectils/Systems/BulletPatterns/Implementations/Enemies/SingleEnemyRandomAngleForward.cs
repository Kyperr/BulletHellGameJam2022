using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SingleEnemyRandomAngleForward", menuName = "ScriptableObjects/BulletPatterns/Enemy/SingleEnemyRandomAngleForward", order = 1)]
public class SingleEnemyRandomAngleForward : EnemyBulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private float shotAngle = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null)
    {
        Vector3 baseDirection = GetBaseDirection(source);

        float randomAngle = UnityEngine.Random.Range(0, shotAngle) - (shotAngle / 2);

        Vector3 spawnDirection1 = Quaternion.AngleAxis(randomAngle, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

        Vector3 spawnPositon1 = source.transform.position + (spawnDirection1 * projectileFireDistance);

        GameObject go1 = Instantiate(projectile, spawnPositon1, source.transform.rotation);
        go1.transform.rotation = Quaternion.LookRotation(spawnDirection1);

        if (go1.GetComponent<Rigidbody>())
        {
            go1.GetComponent<Rigidbody>().velocity = spawnDirection1 * initialProjectileVelocity;
        }

        if (whenDone != null) whenDone();
        yield break;
    }
}
