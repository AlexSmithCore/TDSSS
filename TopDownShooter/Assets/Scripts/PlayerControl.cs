using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private Vector3 _inputs = Vector3.zero;
	Rigidbody rb;
	Animator animator;
	Camera mainCamera;

	public float playerSpeed;



	void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		mainCamera = FindObjectOfType<Camera>();
	}

	void Update()
	{
		 _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
		
		//animator.Play("Base", 0);
		animator.SetFloat("Move", _inputs.magnitude);
		//animator.SetFloat("BackFor", _inputs.z);
		//animator.SetFloat("LeftRight", _inputs.x);

		animator.SetFloat("ForBack", + _inputs.z * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		+ _inputs.x * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));
		animator.SetFloat("LeftRight", _inputs.x * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		-  _inputs.z * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));

		Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLenght;
		if(groundPlane.Raycast(cameraRay, out rayLenght)){
			Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}
	}
	
	void FixedUpdate()
	{
		rb.MovePosition(rb.position + _inputs * playerSpeed * Time.fixedDeltaTime);
	}
}
