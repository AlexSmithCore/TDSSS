using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerThirstSystem : MonoBehaviour {

	public Image stomac;
	public Image kidney;

	public float thirst;
	public float maxThirst;
	public float hunger;
	public float maxHunger;

	void Start(){
		hunger = maxHunger;
		thirst = maxThirst;
	}

	void Update(){
		stomac.fillAmount = hunger / maxHunger;
		kidney.fillAmount = thirst / maxThirst;

		hunger -= 0.1f * Time.deltaTime;
		thirst -= 0.15f * Time.deltaTime;

		if(hunger >= maxHunger){
			hunger = maxHunger;
		}

		if(thirst >= maxThirst){
			thirst = maxThirst;
		}
	}
}
