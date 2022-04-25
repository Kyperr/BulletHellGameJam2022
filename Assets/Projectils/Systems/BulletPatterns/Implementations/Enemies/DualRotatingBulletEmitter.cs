using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DualRotatingBulletEmitter", menuName = "ScriptableObjects/BulletPatterns/Enemy/DualRotatingBulletEmitter", order = 1)]
public class DualRotatingBulletEmitter : EnemyBulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private float numberOfShots = 10f;

    [SerializeField]
    private float rateOfFire = 10f;

    [SerializeField]
    private float angleDelta = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source, Action whenDone, Nullable<Vector3> directionOverride = null)
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

        int shotsFired = 0;

        while (shotsFired < numberOfShots)
        {

            Vector3 spawnDirection1 = new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
            Vector3 spawnDirection2 = new Vector3(baseDirection.x, 0, baseDirection.z).normalized * -1;

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

            baseDirection = Quaternion.AngleAxis(angleDelta, Vector3.up) * baseDirection;

            shotsFired++;
            yield return new WaitForSeconds(60f / rateOfFire);
        }

        if (whenDone != null) whenDone();

        yield break;
    }
}
