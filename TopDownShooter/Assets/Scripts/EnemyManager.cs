using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public int health = 100;

	public Transform attackedBy;
 
	void Update(){
		if(health<=0){
			Destroy(this.gameObject);
		}
	}

	public void Hurt(int damage){
		health-= damage;
	}
}
