using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RadialSprayPlayerBulletPattern", menuName = "ScriptableObjects/BulletPatterns/Player/RadialSprayPlayerBulletPattern", order = 1)]
public class RadialSprayPlayerBulletPattern : BulletPattern
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
        Vector3 mousePos = Input.mousePosition;

        float directionX = mousePos.x - (Screen.width / 2);
        float directionZ = mousePos.y - (Screen.height / 2);

        Vector3 spawnPositon;
        Vector3 spawnDirection = new Vector3(directionX, 0, directionZ).normalized;

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
