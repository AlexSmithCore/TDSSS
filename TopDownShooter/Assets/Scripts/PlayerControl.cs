using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private Vector3 _inputs = Vector3.zero;
	Rigidbody rb;
	Animator animator;
	public float playerSpeed;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		 _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
		
		//animator.Play("Base", 0);
		animator.SetFloat("Move", _inputs.magnitude);
		animator.SetFloat("BackFor", _inputs.z);
		animator.SetFloat("LeftRight", _inputs.x);
	}
	
	void FixedUpdate()
	{
		rb.MovePosition(rb.position + _inputs * playerSpeed * Time.fixedDeltaTime);
	}
}
