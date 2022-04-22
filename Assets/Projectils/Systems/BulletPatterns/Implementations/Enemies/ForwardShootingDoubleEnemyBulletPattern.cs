using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ForwardShootingDoubleEnemyBulletPattern", menuName = "ScriptableObjects/BulletPatterns/Enemy/ForwardShootingDoubleEnemyBulletPattern", order = 1)]
public class ForwardShootingDoubleEnemyBulletPattern : EnemyBulletPattern
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


        Vector3 spawnDirection1 = Quaternion.AngleAxis(-shotAngle / 2, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection2 = Quaternion.AngleAxis(shotAngle / 2, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

        Vector3 spawnPositon1 = source.transform.position + (spawnDirection1 * projectileFireDistance);
        Vector3 spawnPositon2 = source.transform.position + (spawnDirection2 * projectileFireDistance);

        GameObject go1 = Instantiate(projectile, spawnPositon1, source.transform.rotation);
        GameObject go2 = Instantiate(projectile, spawnPositon2, source.transform.rotation);
        go1.transform.rotation = Quaternion.LookRotation(spawnDirection1);
        go2.transform.rotation = Quaternion.LookRotation(spawnDirection2);

        if (go1.GetComponent<Rigidbody>())
        {
            go1.GetComponent<Rigidbody>().velocity = spawnDirection1 * initialProjectileVelocity;
        }

        if (go2.GetComponent<Rigidbody>())
        {
            go2.GetComponent<Rigidbody>().velocity = spawnDirection2 * initialProjectileVelocity;
        }

        if (whenDone != null) whenDone();
        yield break;
    }
}
