using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private int fireRate = 600;// rounds per minute

    [SerializeField]
    private BulletPattern bulletPattern;

    private float timeSinceLastFire = float.MaxValue;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        rb.AddForce((new Vector3(horizontalInput, 0, verticalInput).normalized * moveSpeed) - rb.velocity, ForceMode.VelocityChange);
        //rb.MovePosition(rb.position+ new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);
        // rb.AddForce(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed*10 * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {

        //
        //transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);

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
