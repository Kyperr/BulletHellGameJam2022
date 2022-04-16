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
    private float projectileFireDistance = 1.5f;

    [SerializeField]
    private float initialProjectileVelocity = 10f;

    [SerializeField]
    private GameObject objectToFire;

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

        if (Input.GetButton("Fire1") && timeSinceLastFire > (60f/(float)fireRate) && objectToFire != null)
        {
            SpawnProjectile();
            timeSinceLastFire = 0;
        }
        else
        {
            timeSinceLastFire += Time.deltaTime;
        }
    }

    private void SpawnProjectile()
    {
        Vector3 mousePos = Input.mousePosition;

        float directionX = mousePos.x - (Screen.width / 2);

        // We are translating the y position of the mouse to the z position in the world.
        float directionZ = mousePos.y - (Screen.height / 2);

        // float directionZ = transform.position.y;


        Vector3 spawnDirection = new Vector3(directionX, 0, directionZ).normalized;

        Vector3 spawnPositon = this.transform.position + (spawnDirection * projectileFireDistance);

        GameObject go = Instantiate(objectToFire, spawnPositon, this.transform.rotation);

        if (go.GetComponent<Velocity>())
        {
            go.GetComponent<Velocity>().SetVelocity(spawnDirection * initialProjectileVelocity);
        }

    }
}
