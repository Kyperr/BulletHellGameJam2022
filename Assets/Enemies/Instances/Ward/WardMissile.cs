using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardMissile : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private GameObject target;

    private Rigidbody rb;

    void Start()
    {
        target = EnemySpawner.Instance.Target;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        rb.AddForce(direction * speed);
    }
}
