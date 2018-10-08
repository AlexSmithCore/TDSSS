using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public int health = 100;

	private Animator animator;

	private EnemyController ec;

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
		ec.isStun = true;
		ec.stunCounter = ec.stunTime;
		ec.enemySpeed -= 0.5f;
		animator.SetFloat("isStun", 0.4f);
		if(!ec.isDetected){
			ec.enemyTarget = hittedBy;
			ec.isDetected = true;
		}
	}

	public void Death(){
		GetComponent<ItemDropSystem>().RandomDrop();
		Destroy(this.gameObject);
	}
}
