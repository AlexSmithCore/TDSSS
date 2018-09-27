using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

	public bool isExplode;
	public float distToHit;
	public float maxDamage;
	public float damage;

	public float timeToExplode;
	public float animationTime;

	public GameObject PS_exp;

	public GameObject model;
	public Collider mainCollider;
	public Collider explodeCollider;

	Rigidbody rb;

	TrailRenderer tr;

	void Start(){
		rb = GetComponent<Rigidbody>();
		tr = GetComponent<TrailRenderer>();
		PS_exp.SetActive(isExplode);
		explodeCollider.enabled = isExplode;
	}

	void Update(){
		timeToExplode-=Time.deltaTime;
		if(timeToExplode <= 0 && !isExplode){
			StartCoroutine(Explode());
		}
	}

	IEnumerator Explode(){
		Destroy(rb);
		mainCollider.enabled = false;
		isExplode = true;
		model.SetActive(!isExplode);
		PS_exp.SetActive(isExplode);
		explodeCollider.enabled = isExplode;
		tr.enabled = !isExplode;
		yield return new WaitForSeconds(0.05f);
		explodeCollider.enabled = false;
		StartCoroutine(WaitForAnim());
	}

	IEnumerator WaitForAnim(){
		yield return new WaitForSeconds(animationTime);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(isExplode){
			if(other.tag == "Player"){
				float dist = Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(other.transform.position.x,0,other.transform.position.z));
				if(dist > 2f){
				damage = 100 - ((dist / (distToHit + 2f)) * 100);
				} else {
					damage = 100;
				}
				//other.GetComponent<PlayerManager>().health -= (int)damage;
				print(dist + " " + damage);
			}

			if(other.tag == "Enemy"){
				float dist = Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(other.transform.position.x,0,other.transform.position.z));
				if(dist > 2f){
				damage = 100 - ((dist / (distToHit + 2f)) * 100);
				} else {
					damage = 100;
				}
				//other.GetComponent<ZombieManager>().health -= (int)damage;
				print(dist + " " + damage);
			}
		}
	}
}
