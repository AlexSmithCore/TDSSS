using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float speed;

	public float lifeTime;

	public int damage;

	void Update(){
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	
		lifeTime -=Time.deltaTime;
		if(lifeTime <= 0){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Test!");
		if(other.tag == "Enemy"){
			other.GetComponent<EnemyManager>().Hurt(damage);
			Destroy(gameObject);
		}
	}
}
