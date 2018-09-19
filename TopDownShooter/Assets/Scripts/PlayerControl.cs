using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private Vector3 _inputs = Vector3.zero;
	private Vector3 pointToLook;
	Rigidbody rb;
	Animator animator;
	Camera mainCamera;

	public float playerSpeed;



	void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		mainCamera = FindObjectOfType<Camera>();

		pointToLook = Vector3.zero;
	}

	void Update()
	{
		Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLenght;
		if(groundPlane.Raycast(cameraRay, out rayLenght)){
			pointToLook = cameraRay.GetPoint(rayLenght);
			pointToLook.Set(pointToLook.x, transform.position.y, pointToLook.z);
			transform.LookAt(pointToLook);
		}

		_inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
		
		//animator.Play("Base", 0);
		animator.SetFloat("Move", _inputs.normalized.magnitude);
		//animator.SetFloat("BackFor", _inputs.z);
		//animator.SetFloat("LeftRight", _inputs.x);

		animator.SetFloat("BackFor", + _inputs.normalized.z * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		+ _inputs.normalized.x * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));
		animator.SetFloat("LeftRight", _inputs.normalized.x * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		- _inputs.normalized.z * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));

		
	}
	
	void FixedUpdate()
	{
		rb.MovePosition(rb.position + _inputs * playerSpeed * Time.fixedDeltaTime);
	}
}
