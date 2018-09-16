using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerSystem : MonoBehaviour {

	public Vector3 startScale;
	public Vector3 pointToScale; 

	public float scaleSpeed;

	private Vector3 velocity = Vector3.zero;

	void Start(){
		transform.localScale = startScale;
	}

	void Update(){
		transform.localScale = Vector3.SmoothDamp(transform.localScale, pointToScale, ref  velocity,scaleSpeed * Time.deltaTime);
		if(transform.localScale == pointToScale){
			this.enabled = false;
		}
	}
}
