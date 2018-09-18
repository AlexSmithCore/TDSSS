using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public int health = 100;

	void Update(){
		if(health<=0){
			Destroy(this.gameObject);
		}
	}

	public void Hurt(int damage){
		print("Damaged!");
		health-= damage;
	}
}
