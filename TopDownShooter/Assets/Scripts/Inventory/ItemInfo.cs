﻿using UnityEngine;

public class ItemInfo : MonoBehaviour{

	public Item item;

	public float radius = 3f;
	Transform player;

	bool hasInteracted = false;

	public virtual void Interact(){
		Debug.Log("Interacting with " + transform.name);
	}

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update(){
		if(!hasInteracted){
			float distance = Vector3.Distance(player.transform.position,transform.position);
			if(distance <= radius){
				PickUp();
				hasInteracted = true;
			}
		}
	}

	void PickUp(){
		Debug.Log("Item picked!");
		bool wasPickedUp = Inventory.instance.Add(item);
		if(wasPickedUp){
			Destroy(gameObject);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position + Vector3.up * 1, radius);
	}

}
