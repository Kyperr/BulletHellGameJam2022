using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardShootingTripleEnemyBulletPattern", menuName = "ScriptableObjects/BulletPatterns/Enemy/ForwardShootingTripleEnemyBulletPattern", order = 1)]
public class ForwardShootingTripleEnemyBulletPattern : BulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private float shotAngle = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source)
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


        Vector3 spawnDirection1 = Quaternion.AngleAxis(-shotAngle, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection2 = new Vector3(baseDirection.x, 0, baseDirection.z).normalized;
        Vector3 spawnDirection3 = Quaternion.AngleAxis(shotAngle, Vector3.up) * new Vector3(baseDirection.x, 0, baseDirection.z).normalized;

        Vector3 spawnPositon1 = source.transform.position + (spawnDirection1 * projectileFireDistance);
        Vector3 spawnPositon2 = source.transform.position + (spawnDirection2 * projectileFireDistance);
        Vector3 spawnPositon3 = source.transform.position + (spawnDirection3 * projectileFireDistance);

        GameObject go1 = Instantiate(projectile, spawnPositon1, source.transform.rotation);
        GameObject go2 = Instantiate(projectile, spawnPositon2, source.transform.rotation);
        GameObject go3 = Instantiate(projectile, spawnPositon3, source.transform.rotation);

        go1.transform.rotation = Quaternion.LookRotation(spawnDirection1);
        go2.transform.rotation = Quaternion.LookRotation(spawnDirection2);
        go3.transform.rotation = Quaternion.LookRotation(spawnDirection3);

        if (go1.GetComponent<Rigidbody>())
        {
            go1.GetComponent<Rigidbody>().AddForce(spawnDirection1 * initialProjectileVelocity);
        }

        if (go2.GetComponent<Rigidbody>())
        {
            go2.GetComponent<Rigidbody>().AddForce(spawnDirection2 * initialProjectileVelocity);
        }

        if (go3.GetComponent<Rigidbody>())
        {
            go3.GetComponent<Rigidbody>().AddForce(spawnDirection3 * initialProjectileVelocity);
        }

        yield break;
    }
}
