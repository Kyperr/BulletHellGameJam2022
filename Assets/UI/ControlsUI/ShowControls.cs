using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Update()
    {
        animator.SetBool("Show", Input.GetButton("ShowControls"));
    }
}
