using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEmitter : MonoBehaviour
{

    [SerializeField]
    private float delay = 1f;

    [SerializeField]
    private bool destroyWhenDone = true;

    [SerializeField]
    private bool fireAutomatically = true;

    [SerializeField]
    private EnemyBulletPattern bulletPattern;

    private float timeSinceStarted = 0;

    void Update()
    {
        timeSinceStarted += Time.deltaTime;

        if (fireAutomatically)
        {
            if (timeSinceStarted > delay)
            {
                if (destroyWhenDone)
                {
                    StartCoroutine(bulletPattern.TriggerBulletPattern(this.gameObject, () =>
                    {
                        Destroy(this.gameObject);
                    }));
                }
                else
                {
                    StartCoroutine(bulletPattern.TriggerBulletPattern(this.gameObject));
                }
                this.enabled = false;
            }
        }
    }

    public void Fire()
    {
        if (destroyWhenDone)
        {
            StartCoroutine(bulletPattern.TriggerBulletPattern(this.gameObject, () =>
            {
                Destroy(this.gameObject);
            }));
        }
        else
        {
            StartCoroutine(bulletPattern.TriggerBulletPattern(this.gameObject));
        }
    }

}
