using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {

	[SerializeField]
	ShootingScript ss;

	public int damage;

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy" && ss.isShooting){
			other.GetComponent<ZombieManager>().Hurt(damage);
		}
	}
}
