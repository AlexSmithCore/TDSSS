using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	private bool isHit;

	public float speed;

	public float lifeTime;

	public int damage;

	public GameObject ps_blood;
	private Collider col;
	private MeshRenderer mr;

	void Awake(){
		//ps_blood = transform.GetChild(0).gameObject;
		ps_blood.SetActive(false);
		col = GetComponent<Collider>();
		mr = GetComponent<MeshRenderer>();
	}

	void Update(){
		if(!isHit){
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
	
		lifeTime -=Time.deltaTime;
		if(lifeTime <= 0){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy"){
			isHit = true;
			other.GetComponent<EnemyManager>().Hurt(damage);
			ps_blood.SetActive(true);
			col.enabled = false;
			mr.enabled = false;
			//StartCoroutine(Destroy());
		}
	}

	IEnumerator Destroy(){
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
