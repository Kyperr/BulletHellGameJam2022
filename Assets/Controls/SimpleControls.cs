using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private int fireRate = 600;// rounds per minute

    [SerializeField]
    private BulletPattern bulletPattern;

    private float timeSinceLastFire = float.MaxValue;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);

        if (Input.GetButton("Fire1") && timeSinceLastFire > (60f / (float)fireRate) && bulletPattern != null)
        {
            StartCoroutine(bulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastFire = 0;
        }
        else
        {
            timeSinceLastFire += Time.deltaTime;
        }
    }
}
