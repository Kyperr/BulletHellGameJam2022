using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellingShield : MonoBehaviour
{

    [SerializeField]
    private Vector3 boxSize = new Vector3(1, 1, 1);

    [SerializeField]
    private Transform repelOrigin;

    [SerializeField]
    private Vector3 repelNormal = new Vector3(0, 0, 1);

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = Vector3.Reflect(rb.velocity, (repelOrigin.position - other.transform.position).normalized);
            PopupManager.Instance.createPopupText(transform.position, "Repelled", Color.yellow);
            rb.WakeUp();
        }
    }


}
