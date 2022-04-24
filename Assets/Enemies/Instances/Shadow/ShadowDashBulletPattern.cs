using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TargetDirection))]
public class ShadowDashBulletPattern : EnemyBulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private int numberOfShots = 15;

    [SerializeField]
    private float shotAngle = 15f;

    [SerializeField]
    private float delayBetweenShots = .1f;

    public override IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null)
    {
        Vector3 baseDirection = GetBaseDirection(source);

        Vector3 spawnPositon;
        Vector3 spawnDirection = new Vector3(baseDirection.x, 0, baseDirection.y).normalized;

        for (int i = 0; i < numberOfShots; i++)
        {
            spawnPositon = source.transform.position + (spawnDirection * projectileFireDistance);
            GameObject go = Instantiate(projectile, spawnPositon, source.transform.rotation);

            go.transform.rotation = Quaternion.LookRotation(spawnDirection);

            if (go.GetComponent<Rigidbody>())
            {
                go.GetComponent<Rigidbody>().AddForce(spawnDirection * initialProjectileVelocity);
            }

            yield return new WaitForSeconds(delayBetweenShots);

            // After waiting, set the new angle.
            spawnDirection = Quaternion.AngleAxis(shotAngle, Vector3.up) * spawnDirection;
        }

        if (whenDone != null) whenDone();
        yield break;
    }
}
