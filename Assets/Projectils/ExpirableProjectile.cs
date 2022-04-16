using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpirableProjectile : MonoBehaviour
{

    [SerializeField]
    private float lifeTimeInSeconds = 10f;

    private float timeAlive = 0f;

    void Update()
    {
        if (lifeTimeInSeconds < 0)
        {
            return;
        }
        timeAlive += Time.deltaTime;

        if (lifeTimeInSeconds <= timeAlive)
        {
            Destroy(this.gameObject);
        }
    }
}
