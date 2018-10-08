using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	#region Variables
		public Transform m_Target;
		[SerializeField]
		private float m_Height = 10f;
		[SerializeField]
		private float m_Distance = 20f;
		[SerializeField]
		private float m_Angle = 45f;

		[SerializeField]
		private float m_Smooth;
		
		private Vector3 velocity = Vector3.zero;
	#endregion


	public bool freeze = false;

	public Transform intermediatePoint;

	public Transform interactionPoint;

	Camera thisCam;

	Vector3 sumOfVectors;

	#region MainMethods
	void Start(){
		HandleCamera();
	}

	void FixedUpdate(){
		HandleCamera();
		/*if(isInteraction){
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
		}*/
	}
	#endregion

	protected virtual void HandleCamera(){
		if(!m_Target){
			return;
		}

		Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
		Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
		Vector3 flatTargetPosition = m_Target.position;
		flatTargetPosition.y = 0;
		Vector3 finalPosition = flatTargetPosition + rotatedVector;
		transform.position = Vector3.SmoothDamp(transform.position,finalPosition, ref velocity, m_Smooth);
		transform.LookAt(m_Target.position);
	}
}
