using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public bool freeze = false;

	public bool isInteraction = false;

	public Transform target;

	public Transform intermediatePoint;

	public Transform interactionPoint;

	public float smooth;
	public Vector3 offSet;

	Camera thisCam;

	Vector3 sumOfVectors;

	PlayerControl pc;

	private Vector3 velocity = Vector3.zero;

	void Start(){
		thisCam = GetComponent<Camera>();
		pc = target.GetComponent<PlayerControl>();
	}

	void FixedUpdate(){
		if(isInteraction){
			transform.position = Vector3.SmoothDamp(transform.position, interactionPoint.position, ref velocity, smooth);
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,0,0), smooth);
			transform.LookAt(target.transform.position + Vector3.up * 1f);
			return;
		}

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
		//transform.LookAt(target);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(60, 0, 0), smooth * Time.fixedUnscaledDeltaTime * 5f);
		}
	}
}
