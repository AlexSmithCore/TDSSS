using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
	public bool isAim;

	public bool isFreeze;

	public BulletController bullet;

	public float bulletSpeed;
	public float interval;
	float shotCounter;
	
	public Transform firePoint;

	public Light shootLight;

	public GameObject PS_shoot;

	public Vector3 _inputs = Vector3.zero;
	private Vector3 pointToLook;
	Rigidbody rb;
	Animator animator;
	Camera mainCamera;

	public float playerSpeed;

	public float walkSpeed;

	public int[] simple;

	public string[] itemName;

	public GameObject sleevesPoint;
	public GameObject sleeve;

	public float spreadSize;

	Inventory inventory;

	Slot currentAmmo;

	public Item gunAmmo;

	public int ammoCount;

	public GameObject weaponPanel;

	void Start()
	{
		inventory = Inventory.instance;
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		mainCamera = FindObjectOfType<Camera>();

		pointToLook = Vector3.zero;

		playerSpeed = walkSpeed;

		Invoke("ShootEffect", .1f);
		UpdateWeaponUI();
	}

	void FixedUpdate()
	{
		if(!isFreeze){
			rb.MovePosition(rb.position + _inputs * playerSpeed * Time.fixedDeltaTime);
		}
	}

	void Update()
	{
		if(!isFreeze){
			Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			float rayLenght;
			if(groundPlane.Raycast(cameraRay, out rayLenght)){
				pointToLook = cameraRay.GetPoint(rayLenght);
				pointToLook.Set(pointToLook.x, transform.position.y, pointToLook.z);
				transform.LookAt(pointToLook);
			}

			_inputs = Vector3.zero;
			_inputs.x = Input.GetAxis("Horizontal");
			_inputs.z = Input.GetAxis("Vertical");
			
			animator.SetFloat("Move", _inputs.normalized.magnitude);

			animator.SetFloat("BackFor", + _inputs.normalized.z * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
			+ _inputs.normalized.x * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));
			animator.SetFloat("LeftRight", _inputs.normalized.x * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
			- _inputs.normalized.z * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));

			isAim = Input.GetMouseButton(1);

// Reload
		if(Input.GetKeyDown(KeyCode.R)){
			if(Reload(gunAmmo)){
				Debug.Log("Reloaded!");
			} else {
				Debug.Log("Don't have any ammo!");
			}
		}

		if(Input.GetMouseButtonDown(0)){
			if(isAim && ammoCount > 0){
			//shotCounter -= Time.deltaTime;
			currentAmmo.count--;
			UpdateWeaponUI();
			shootLight.enabled = true;
			PS_shoot.SetActive(true);
			Invoke("ShootEffect", .1f);
			shotCounter = interval - Random.Range(0f,0.3f);
			float xSpread = Random.Range(-1, 1);
			float ySpread = Random.Range(-1, 1);
			Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * spreadSize;
			Quaternion rotation = Quaternion.Euler(spread) * transform.rotation;
			BulletController newBullet = Instantiate(bullet, firePoint.position, rotation) as BulletController;
			newBullet.speed = bulletSpeed;
			newBullet.parent = transform;
			GameObject newSleeve = Instantiate(sleeve, sleevesPoint.transform.position, Random.rotation);
			newSleeve.GetComponent<Rigidbody>().AddForce(transform.right * 64);
			}
		}

		RandomDrop();
		}
	}

	void RandomDrop(){
		if(Input.GetKeyDown(KeyCode.R)){
			float result = 0;
			int greatest = 0;
			float rand = (float)Random.Range(11, 53);
			for(int i = 0; i < simple.Length; i++){
				result = rand / simple[i]; 
				if (result - Mathf.Floor(result) == 0) 
				{ 
					greatest = i; 
				}
			}

			Debug.Log("Выпало: " + itemName[greatest]);
		}	
	}

	void ShootEffect(){
		PS_shoot.SetActive(false);
		shootLight.enabled = false;
	}

	private bool Reload(Item needBullets){
		for(int i = 0; i < inventory.items.Count; i++){
			if(inventory.items[i].item == needBullets){
				if(currentAmmo != null){
					if(currentAmmo.count > 0)
					inventory.Add(currentAmmo.item, currentAmmo.count);
				}
				currentAmmo = inventory.items[i];
				UpdateWeaponUI();
				inventory.RemoveAllItem(i);
				return true;
			}
		}
		return false;
	}

	private void UpdateWeaponUI(){
		if(currentAmmo != null){
			ammoCount = currentAmmo.count;
		}
		weaponPanel.transform.GetChild(4).GetComponent<Text>().text = ammoCount + "";
		if(ammoCount <= 0){
			weaponPanel.transform.GetChild(6).gameObject.SetActive(true);
			weaponPanel.transform.GetChild(5).gameObject.SetActive(false);
			return;
		}

		weaponPanel.transform.GetChild(6).gameObject.SetActive(false);
		weaponPanel.transform.GetChild(5).gameObject.SetActive(true);
		for(int i = 0; i < weaponPanel.transform.GetChild(5).childCount; i++){
			if(i >= ammoCount){
				weaponPanel.transform.GetChild(5).GetChild(i).gameObject.SetActive(false);
			} else {
				weaponPanel.transform.GetChild(5).GetChild(i).gameObject.SetActive(true);
			}
		}
	}
	
}
