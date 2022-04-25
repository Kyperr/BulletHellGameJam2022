using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProp : MonoBehaviour
{

    [SerializeField]
    private Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotate * Time.deltaTime);
    }
}
