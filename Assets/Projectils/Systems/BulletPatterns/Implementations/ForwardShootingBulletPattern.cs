using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardShootingBulletPattern", menuName = "ScriptableObjects/BulletPatterns/ForwardShootingSingle", order = 1)]
public class ForwardShootingBulletPattern : BulletPattern
{

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    public override IEnumerator TriggerBulletPattern(GameObject source)
    {
        Vector3 mousePos = Input.mousePosition;

        float directionX = mousePos.x - (Screen.width / 2);

        // We are translating the y position of the mouse to the z position in the world.
        float directionZ = mousePos.y - (Screen.height / 2);

        // float directionZ = transform.position.y;

        Vector3 spawnDirection = new Vector3(directionX, 0, directionZ).normalized;

        Vector3 spawnPositon = source.transform.position + (spawnDirection * projectileFireDistance);

        GameObject go = Instantiate(projectile, spawnPositon, source.transform.rotation);

        go.transform.rotation = Quaternion.LookRotation(spawnDirection);

        if (go.GetComponent<Rigidbody>())
        {
            go.GetComponent<Rigidbody>().AddForce(spawnDirection * initialProjectileVelocity);
        }

        yield break;
    }
}
