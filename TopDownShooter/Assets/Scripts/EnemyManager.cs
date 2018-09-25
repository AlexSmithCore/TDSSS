using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public int health = 100;

	private Animator animator;

	public EnemyController ec;

	void Start(){
		ec = GetComponent<EnemyController>();
		animator = GetComponent<Animator>();
	}
 
	void Update(){
		if(health<=0){
		}
	}

	public void Hurt(int damage, Transform hittedBy){
		health-= damage;
		animator.Play("Hit", 0);
		if(!ec.isDetected){
			ec.enemyTarget = hittedBy;
			ec.isDetected = true;
		}
	}

	public void Death(){
		Destroy(this.gameObject);
	}
}
