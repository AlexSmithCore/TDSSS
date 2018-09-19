using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform target;

	public Transform intermediatePoint;

	public float smooth;
	public Vector3 offSet;

	Camera thisCam;

	Vector3 sumOfVectors;

	//ShootingScript ss;

	private Vector3 velocity = Vector3.zero;

	void Start(){
		thisCam = GetComponent<Camera>();
		//ss = target.GetComponent<ShootingScript>();
	}

	void FixedUpdate(){
		/*if(ss.aim){
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
		}*/
		intermediatePoint.position = target.position;
		Vector3 desiredPosition = intermediatePoint.position + offSet;
		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smooth);
	}
}
