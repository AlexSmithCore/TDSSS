using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour {

	public bool aim;
	public bool isShooting;
	public bool isGrenade;
	public bool isMelee;

	public float maxThrowForce;
	public float throwForce;

	public float throwProgress;

	public BulletController bullet;
	public Transform grenadeObj;

	public GameObject grenadeHolder;

	public float bulletSpeed;

	public float interval;

	float shotCounter;

	public Transform firePoint;
	public Transform throwPoint;

	public Light shootLight;

	public GameObject PS_shoot;

	PlayerManager pm;

	PlayerController pc;

	InventoryController ic;

	GameController gc;

	void Start(){
		pm = GetComponent<PlayerManager>();
		pc = GetComponent<PlayerController>();
		ic = GetComponent<InventoryController>();
		gc = FindObjectOfType<GameController>();
	}

	void Update(){
		if(!isMelee){
			pm.ammoText.gameObject.SetActive(true);
			pm.clipsText.gameObject.SetActive(true);
			pm.weaponType.sprite = gc.weaponType[0];
			if(pm.ammo > 0){
			if(isShooting){
				shotCounter -= Time.deltaTime;
				if(shotCounter <= 0){
					shootLight.enabled = true;
					PS_shoot.SetActive(true);
					shotCounter = interval;
					BulletController newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as BulletController;
					newBullet.speed = bulletSpeed;
					pm.ammo--;
					ic.UpdateInvInfo();
					StartCoroutine(ShootEffect());
				}
			} else {
				PS_shoot.SetActive(false);
				shotCounter = 0;
			}
			} else {
				PS_shoot.SetActive(false);
			}

			if(pm.ammo > 0 && aim){
				isShooting = Input.GetMouseButton(0);
			} else {
				isShooting = false;
			}
		} else {
			if(aim){
					isShooting = Input.GetMouseButton(0);
			} else {
				isShooting = false;
			}

			pm.ammoText.gameObject.SetActive(false);
			pm.clipsText.gameObject.SetActive(false);
			pm.weaponType.sprite = gc.weaponType[1];
		}


		if(!pc.isReloading){
			aim = Input.GetMouseButton(1);
		}
		if(aim){
			pc.isRunning = false;
		}
		
		if(ic.grenadeCount > 0){
			isGrenade = Input.GetKey(KeyCode.G);
			if(Input.GetKeyUp(KeyCode.G) && throwForce >= 16){
				Throw();
			}

			if(isGrenade){
				if(throwForce < maxThrowForce){
					throwForce+=Time.deltaTime * 64;
					throwProgress= throwForce / maxThrowForce;
				} else {
					Throw();
					throwForce = 0;
					throwProgress = 1;
				}
			} else {
				throwProgress = 0;
				throwForce = 0;
			}
		}
	}

	IEnumerator ShootEffect(){
		yield return null;
		shootLight.enabled = false;
	}

	void Throw(){
		ic.grenadeCount--;
		isGrenade = false;
		Transform throwObj = Instantiate(grenadeObj, throwPoint.position, Quaternion.identity);
		throwObj.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
	}
}
