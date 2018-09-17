using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

	public bool isDead;

	public int maxHealth;
	public int health;

	public Transform[] supplyDrop;

	GameController gc;

	ZombieController zc;

	void Start(){
		zc = GetComponent<ZombieController>();
		gc = FindObjectOfType<GameController>();
		health = maxHealth;
	}

	void Update(){
		if(health <= 0 && !isDead){
			isDead = true;
			zc.anim.SetTrigger("death");
			Invoke("Death", 2f);
		}
	}

	public void Hurt(int damage){
		zc.StopAllCoroutines();
		zc.isStun = true;
		zc.StartCoroutine(zc.WaitToStun());
		health -= damage;

	}

	public void DropSupply(){
			int rand = Random.Range(0,10);

			switch(rand){
				case 7:
				rand = 0;
				break;
				case 8:
				rand = 1;
				break;
				case 9:
				rand = 1;
				break;
				case 10:
				rand = 1;
				break;
			}
			if(rand > 0){
			Transform supply = Instantiate(supplyDrop[rand],new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
			switch(rand){
				case 3:
				supply.gameObject.name = "GrenadeDrop";
				break;
				case 2:
				supply.gameObject.name = "MedKitDrop";
				break;
				case 1:
				supply.gameObject.name = "AmmoDrop";
				break;
			}
		}
	}


	private void Death(){
		DropSupply();
		Destroy(gameObject);
	}
}
