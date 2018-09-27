using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public bool freeze = false;

	public Transform target;

	public Transform intermediatePoint;

	public float smooth;
	public Vector3 offSet;
	private Vector3 startOffset;

	Camera thisCam;

	Vector3 sumOfVectors;

	PlayerControl pc;

	private Vector3 velocity = Vector3.zero;

	//public GameController gc;

	void Start(){
		thisCam = GetComponent<Camera>();
		pc = target.GetComponent<PlayerControl>();

		//gc = (GameController)FindObjectOfType(typeof(GameController));
	}

	void FixedUpdate(){
		if(!freeze){
		if(pc.isAim){
			Ray cameraRay = thisCam.ScreenPointToRay(Input.mousePosition);
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			float rayLenght;
			if(groundPlane.Raycast(cameraRay, out rayLenght)){
				Vector3 rayPoint = cameraRay.GetPoint(rayLenght);
				sumOfVectors = (new Vector3(target.position.x, target.transform.position.y, target.position.z) + new Vector3(rayPoint.x,0,rayPoint.z)) / 2;
				intermediatePoint.position = sumOfVectors;
			}
		} else {
			intermediatePoint.position = target.position;
		}
		Vector3 desiredPosition = intermediatePoint.position + offSet;
		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smooth);
	}
	}
}
