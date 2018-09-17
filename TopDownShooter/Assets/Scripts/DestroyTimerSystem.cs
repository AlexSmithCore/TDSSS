using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimerSystem : MonoBehaviour {


	public bool isBlood;
	public float timeToDestroy;

	private float timer;

	void Start(){
		timer = 0;
	}

	void Update(){
		timer+=Time.deltaTime;
		if(isBlood && timer >= timeToDestroy){
			GetComponent<Renderer>().material.color -= new Color(0,0,0,1 * Time.deltaTime);
			if(timer >= timeToDestroy + 2f){
				Destroy(gameObject);
			}
		}
	}
}
