using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour {

	public bool isPicked;

	[System.Serializable]
	public struct IInfo{
		public string itemName;

		public int itemID;

		public float pickTime;
	}

	public IInfo itemInfo;

	GameObject target;

	Vector3 scaleVelocity = Vector3.zero;

	void Start(){
		target = FindObjectOfType<PlayerManager>().gameObject;
	}

	void Update(){
		if(isPicked){
			transform.localScale = Vector3.SmoothDamp(transform.localScale, new Vector3(0,0,0), ref scaleVelocity, 6f * Time.deltaTime);
			if(transform.localScale.x <= 0.2f){
				Destroy(this.gameObject);
			}
		}
	}
}
