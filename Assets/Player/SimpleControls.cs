using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SimpleControls : MonoBehaviour
{
    [SerializeField]
    public Texture2D mouseCursor;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private int primaryFireRate = 200;// rounds per minute

    [SerializeField]
    private BulletPattern primaryBulletPattern;

    [SerializeField]
    private int altFireRate = 10;// rounds per minute

    [SerializeField]
    private BulletPattern altBulletPattern;

    private float timeSinceLastPrimaryFire = float.MaxValue;
    private float timeSinceLastAltFire = float.MaxValue;

    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    private void Awake()
    {
        Cursor.SetCursor(mouseCursor, new Vector2(0,0), CursorMode.Auto);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput, 0, verticalInput).normalized * moveSpeed;
        //rb.MovePosition(rb.position+ new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);
        // rb.AddForce(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed*10 * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        timeSinceLastPrimaryFire += Time.deltaTime;
        timeSinceLastAltFire += Time.deltaTime;
        //
        //transform.Translate(new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime);

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButton("Fire1") && timeSinceLastPrimaryFire > (60f / (float)primaryFireRate) && primaryBulletPattern != null)
        {
            StartCoroutine(primaryBulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastPrimaryFire = 0;
        }

        if (Input.GetButton("Fire2") && timeSinceLastAltFire > (60f / (float)altFireRate) && primaryBulletPattern != null)
        {
            StartCoroutine(altBulletPattern.TriggerBulletPattern(this.gameObject));
            timeSinceLastAltFire = 0;
        }
    }

    public float GetAltCooldown()
    {
        return Mathf.Clamp(timeSinceLastAltFire / (60f / altFireRate), 0f, 1f);
    }
}
