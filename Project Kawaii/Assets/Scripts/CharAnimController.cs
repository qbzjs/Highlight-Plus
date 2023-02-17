using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimController : MonoBehaviour
{
    public float movement = 0;
    public bool jump = false;
    public bool dash = false;
    public bool isMoving = false;
    public bool isGrounded = true;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        OnAnimChange();
    }

    public void OnAnimChange()
    {
        anim.SetFloat("Movement", movement, 0.05f, Time.deltaTime);
        if (jump)
        {
            anim.SetTrigger("Jump");
            jump = false;
        }
        if (dash)
        {
            anim.SetTrigger("Dash");
            dash = false;
        }
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }
}