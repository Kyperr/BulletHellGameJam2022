using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ForwardShootingEnemyBulletPattern", menuName = "ScriptableObjects/BulletPatterns/Enemy/ForwardShootingEnemyBulletPattern", order = 1)]
public class ForwardShootingEnemyBulletPattern : EnemyBulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null)
    {
        Vector3 baseDirection = new Vector3();

        Target target = source.GetComponent<Target>();
        if (target)
        {
            baseDirection = target.GetTarget().transform.position - source.transform.position;
        }
        else
        {
            baseDirection = source.transform.rotation.eulerAngles - source.transform.position;
        }

        Vector3 spawnDirection = new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

        Vector3 spawnPositon = source.transform.position + (spawnDirection * projectileFireDistance);

        GameObject go = Instantiate(projectile, spawnPositon, source.transform.rotation);

        go.transform.rotation = Quaternion.LookRotation(spawnDirection);

        if (go.GetComponent<Rigidbody>())
        {
            go.GetComponent<Rigidbody>().AddForce(spawnDirection * initialProjectileVelocity);
        }

        if(whenDone != null) whenDone();
        yield break;
    }
}
