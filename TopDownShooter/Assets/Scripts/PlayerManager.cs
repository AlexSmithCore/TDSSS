using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

	public int maxHealth;
	public float health;

	public int ammo;
	public int clips;
	public int clipsLenght;

	public float reloadingTime;

	public Text ammoText;
	public Text clipsText;
	public Text grenadeCount;

	public Image grenade;

	public Image weaponType;

	GameController gc;

	ShootingScript ss;

	InventoryController ic;

	void Start(){
		gc = FindObjectOfType<GameController>();
		ic = GetComponent<InventoryController>();
		ss = GetComponent<ShootingScript>();
		health = maxHealth;
	}

	void Update(){
		ammoText.text = "" + ammo;
		clipsText.text = "" + clips;
		grenade.fillAmount = ss.throwProgress;
		grenadeCount.text = "" + ic.grenadeCount;
		if(health <= 0){
			gc.isDead=true;
			this.gameObject.SetActive(false);
		}
	}
}
