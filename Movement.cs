using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class Movement : MonoBehaviour {

    Animator anim;
    bool isWalking = false;
    const float WALK_SPEED = .1f;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Walking();
        Turning();
        Move();
        Jump();
	}

    void Turning()
    {
        anim.SetFloat("Turn", Input.GetAxis("Horizontal"));
    }

    void Walking()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isWalking = !isWalking;
            anim.SetBool("Walk", isWalking);
        }
    }

    void Move()
    {
        if (anim.GetBool("Walk"))
        {
            anim.SetFloat("Forward", Mathf.Clamp(Input.GetAxis("Vertical"), 0, WALK_SPEED));
        }
        else
        {
            anim.SetFloat("Forward", Input.GetAxis("Vertical"));
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }
    }
}
