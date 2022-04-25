using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ForwardShootingFiveEnemyBulletPattern", menuName = "ScriptableObjects/BulletPatterns/Enemy/ForwardShootingFiveEnemyBulletPattern", order = 1)]
public class ForwardShootingFiveEnemyBulletPattern : EnemyBulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private float shotAngle = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null, Nullable<Vector3> directionOverride = null)
    {
        Vector3 baseDirection = GetBaseDirection(source, directionOverride);

        Vector3 spawnDirection1 = Quaternion.AngleAxis(-shotAngle * 2, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection2 = Quaternion.AngleAxis(-shotAngle, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection3 = new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection4 = Quaternion.AngleAxis(shotAngle, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection5 = Quaternion.AngleAxis(shotAngle * 2, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

        Vector3 spawnPositon1 = source.transform.position + (spawnDirection1 * projectileFireDistance);
        Vector3 spawnPositon2 = source.transform.position + (spawnDirection2 * projectileFireDistance);
        Vector3 spawnPositon3 = source.transform.position + (spawnDirection3 * projectileFireDistance);
        Vector3 spawnPositon4 = source.transform.position + (spawnDirection4 * projectileFireDistance);
        Vector3 spawnPositon5 = source.transform.position + (spawnDirection5 * projectileFireDistance);

        GameObject go1 = Instantiate(projectile, spawnPositon1, source.transform.rotation);
        GameObject go2 = Instantiate(projectile, spawnPositon2, source.transform.rotation);
        GameObject go3 = Instantiate(projectile, spawnPositon3, source.transform.rotation);
        GameObject go4 = Instantiate(projectile, spawnPositon4, source.transform.rotation);
        GameObject go5 = Instantiate(projectile, spawnPositon5, source.transform.rotation);

        go1.transform.rotation = Quaternion.LookRotation(spawnDirection1);
        go2.transform.rotation = Quaternion.LookRotation(spawnDirection2);
        go3.transform.rotation = Quaternion.LookRotation(spawnDirection3);
        go4.transform.rotation = Quaternion.LookRotation(spawnDirection4);
        go5.transform.rotation = Quaternion.LookRotation(spawnDirection5);

        if (go1.GetComponent<Rigidbody>())
        {
            go1.GetComponent<Rigidbody>().velocity = spawnDirection1 * initialProjectileVelocity;
        }

        if (go2.GetComponent<Rigidbody>())
        {
            go2.GetComponent<Rigidbody>().velocity = spawnDirection2 * initialProjectileVelocity;
        }

        if (go3.GetComponent<Rigidbody>())
        {
            go3.GetComponent<Rigidbody>().velocity = spawnDirection3 * initialProjectileVelocity;
        }

        if (go4.GetComponent<Rigidbody>())
        {
            go4.GetComponent<Rigidbody>().velocity = spawnDirection4 * initialProjectileVelocity;
        }

        if (go5.GetComponent<Rigidbody>())
        {
            go5.GetComponent<Rigidbody>().velocity = spawnDirection5 * initialProjectileVelocity;
        }

        if (whenDone != null) whenDone();
        yield break;
    }
}
