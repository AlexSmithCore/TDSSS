using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	public bool isOpen;

	public int selectedWeapon;

	public int grenadeCount;

	public Image sWeaponSprite;

	private GameObject gunFolder;

	public WInventory[] wInv;

	private GameObject prevWeapon;

	public Image[] oWeaponSprite;
	
	PlayerManager pm;

	PlayerController pc;

	ShootingScript ss;

	public GameObject inventoryUI;

	void Start(){
		gunFolder = GameObject.Find("GunFolder");
		selectedWeapon = 2;
		prevWeapon = gunFolder.transform.GetChild(1).gameObject;
		pm = GetComponent<PlayerManager>();
		pc = GetComponent<PlayerController>();
		ss = GetComponent<ShootingScript>();
		
		SelectWeapon();
	}

	
	void Update(){
		if(Input.GetKeyDown(KeyCode.C) && !pc.isReloading){
			if(selectedWeapon < 2){
				prevWeapon = FindWeapon();
				selectedWeapon++;
				SelectWeapon();
			} else {
				prevWeapon = FindWeapon();
				selectedWeapon = 0;
				SelectWeapon();
			}
		}

		if(Input.GetKeyDown(KeyCode.I)){
			isOpen = !isOpen;
		}

		inventoryUI.SetActive(isOpen);
	}

	void SelectWeapon(){
		int test = selectedWeapon;
		for(int s = 0; s < 2; s++){
			if(test < 2){
				test++;
			} else {
				test = 0;
			}
			oWeaponSprite[s].sprite = wInv[test].wSprite;
		}

		sWeaponSprite.sprite = wInv[selectedWeapon].wSprite;

		for(int i = 0; i < gunFolder.transform.childCount; i++){
			if(gunFolder.transform.GetChild(i).name == wInv[selectedWeapon].wName){
				gunFolder.transform.GetChild(i).gameObject.SetActive(true);
				if(selectedWeapon != 0){
					ss.firePoint = gunFolder.transform.GetChild(i).GetChild(0).Find("FirePoint");
					ss.PS_shoot = gunFolder.transform.GetChild(i).GetChild(0).Find("FirePoint").GetChild(0).gameObject;
				}
				pc.anim = gunFolder.transform.GetChild(i).GetComponent<Animator>();
				prevWeapon.SetActive(false);
			}
		}

		pm.ammo = wInv[selectedWeapon].wAmmo;
		pm.clips = wInv[selectedWeapon].wClips;
		pm.clipsLenght = wInv[selectedWeapon].wClipsLenght;
		pm.reloadingTime = wInv[selectedWeapon].reloadingTime;
		ss.interval = wInv[selectedWeapon].wInterval;
		if(selectedWeapon == 0){
			ss.isMelee = true;
		} else {
			ss.isMelee = false;

		}
	}

	public GameObject FindWeapon(){
		for(int i = 0; i < gunFolder.transform.childCount; i++){
			if(gunFolder.transform.GetChild(i).name == wInv[selectedWeapon].wName){
				return gunFolder.transform.GetChild(i).gameObject;
			}
		}
		return null;
	}

	public void UpdateInvInfo(){
		wInv[selectedWeapon].wAmmo = pm.ammo;
		pm.clips = wInv[selectedWeapon].wClips;
		pm.clipsLenght = wInv[selectedWeapon].wClipsLenght;
		pm.reloadingTime = wInv[selectedWeapon].reloadingTime;
	}
}

[System.Serializable]
public class WInventory{	
	public string wName;
	public int wID;
	public int wAmmo;
	public int wClips;
	public int wClipsLenght;
	public float reloadingTime;

	public float wDamage;
	public float wInterval;

	public Sprite wSprite;
}

