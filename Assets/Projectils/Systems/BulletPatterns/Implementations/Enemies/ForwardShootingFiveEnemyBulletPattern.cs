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
    private float shotDelay = .1f;

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

        SpawnAndSend(source, spawnDirection1);
        yield return new WaitForSeconds(shotDelay);
        SpawnAndSend(source, spawnDirection2);
        yield return new WaitForSeconds(shotDelay);
        SpawnAndSend(source, spawnDirection3);
        yield return new WaitForSeconds(shotDelay);
        SpawnAndSend(source, spawnDirection4);
        yield return new WaitForSeconds(shotDelay);
        SpawnAndSend(source, spawnDirection5);
        yield return new WaitForSeconds(shotDelay);

        if (whenDone != null) whenDone();
        yield break;
    }

    private void SpawnAndSend(GameObject source, Vector3 spawnDirection)
    {
        Vector3 spawnPositon = source.transform.position + (spawnDirection * projectileFireDistance);
        GameObject go = Instantiate(projectile, spawnPositon, source.transform.rotation);
        go.transform.rotation = Quaternion.LookRotation(spawnDirection);
        if (go.GetComponent<Rigidbody>())
        {
            go.GetComponent<Rigidbody>().velocity = spawnDirection * initialProjectileVelocity;
        }

    }
}
