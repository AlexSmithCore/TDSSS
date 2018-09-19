using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public int health = 100;

	public Transform attackedBy;

	private Animator animator;

	void Start(){
	}
 
	void Update(){
		if(health<=0){
		}
	}

	public void Hurt(int damage){
		health-= damage;
	}

	public void Death(){
		Destroy(this.gameObject);
	}
}
