using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class PlayerController : MonoBehaviour {

	public Animator anim;

	public bool isMoving;
	public bool isRunning;
	public bool isRecovering;
	public bool isReloading;

	public float playerSpeed;
	public float gravity;
	[Space]
	public float runSpeed;
	public float normalSpeed;
	public float aimSpeed;
	public float recoveringSpeed;
	[Space]
	public float stamina;
	public float maxStamina;
	[Space]
	public float staminaCost;
	public float staminaRecovery;

    public float GroundDistance = 0.2f;
	public LayerMask Ground;

    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

	ShootingScript ss;

	CharacterController cc;
	PlayerManager pm;

	Rigidbody rb;
	
	Camera mainCamera;

	InventoryController ic;

	public GameObject itemInfoPanel;

	public Text pickText;

	public Slider pickSlider;

	public Gradient pickColor;

	private float picking;

	public PostProcessingProfile ppp;

	BloodSystem bs;

	HungerThirstSystem hts;

	void Start(){
		_groundChecker = transform.GetChild(0);
		rb = GetComponent<Rigidbody>();
		bs = GetComponent<BloodSystem>();
		hts = GetComponent<HungerThirstSystem>();
		ss=GetComponent<ShootingScript>();
		pm = GetComponent<PlayerManager>();
		cc = GetComponent<CharacterController>();
		ic = GetComponent<InventoryController>();
		mainCamera = FindObjectOfType<Camera>();

		stamina = maxStamina;
	}

	void Update(){
		playerSpeed = Mathf.Lerp(playerSpeed, GetSpeed(), 2 * Time.deltaTime);

		_isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

		/*Vector3 playerMovement = new Vector3(h,-gravity,v) * playerSpeed * Time.deltaTime;
		
		cc.Move(playerMovement);
		if(playerMovement != Vector3.zero){
			//transform.forward = playerMovement;
		}*/

		if(stamina > 0 && !ss.aim && isMoving){
			isRunning = Input.GetKey(KeyCode.LeftShift);
		} else if(stamina <= 0) {
			isRunning = false;
			isRecovering = true;
		}

		if(isRecovering){
			StartCoroutine(Recovering());
		}

		if(isRunning){
			if(stamina > 0 && !isRecovering){
				stamina -= staminaCost * Time.deltaTime;
			}
		} else {
			if(stamina < maxStamina && !isRecovering){
				stamina += staminaRecovery * Time.deltaTime;
			}
		}

		if((_inputs.x > 0 || _inputs.x < 0) || (_inputs.z > 0 || _inputs.z < 0)){
			isMoving = true;
		} else {
			isMoving = false;
		}

		anim.SetBool("move", isMoving);
		anim.SetBool("shoot", ss.isShooting);
		anim.SetBool("aim",ss.aim);

		if(Input.GetKeyDown(KeyCode.R) && pm.clips > 0){
			ss.aim = false;
			anim.SetTrigger("reload");
			StartCoroutine(Reload());
		}
		
		VignetteModel.Settings vignetteSettings = ppp.vignette.settings;

		vignetteSettings.intensity = 0.75f - ( 0.25f * (stamina / maxStamina));
		vignetteSettings.roundness = 1f - ( 0.4f * (stamina / maxStamina));

		ppp.vignette.settings = vignetteSettings;

		Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLenght;
		if(groundPlane.Raycast(cameraRay, out rayLenght)){
			Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Supply"){
			ShowItemInfo(other.gameObject, true);
		}
		/*if(other.tag == "Supply" && other.name == "AmmoDrop"){
			Destroy(other.gameObject);
			ic.wInv[ic.selectedWeapon].wClips++;
			ic.UpdateInvInfo();
		}

		if(other.tag == "Supply" && other.name == "MedKitDrop"){
			if(pm.health < 100){
				pm.health+=5;
				if(pm.health > 100){
					pm.health = 100;
				}
				Destroy(other.gameObject);
			}
		}

		if(other.tag == "Supply" && other.name == "GrenadeDrop"){
				Destroy(other.gameObject);
				ic.grenadeCount++;
		}*/

		if(other.tag == "ZombieHand" && other.transform.GetComponentInParent<ZombieController>().isAttacking){
			int rand = Random.Range(0, 100);
			if(rand > 70){
				bs.bleedingCount++;
			}
			bs.bloodCount-= Random.Range(100, 200);
			other.transform.GetComponentInParent<ZombieController>().isAttacking = false;
		}
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Supply"){
			itemInfoPanel.transform.position = mainCamera.WorldToScreenPoint(new Vector3(other.transform.position.x,other.transform.position.y,other.transform.position.z));
			itemInfoPanel.transform.Find("ItemName").GetComponent<Text>().text = other.GetComponent<ItemInfo>().itemInfo.itemName;
			if(Input.GetKey(KeyCode.E)){
				pickSlider.gameObject.SetActive(true);
				picking+=Time.deltaTime;
				pickText.color = pickColor.Evaluate(picking / other.GetComponent<ItemInfo>().itemInfo.pickTime);
				pickSlider.value = picking / other.GetComponent<ItemInfo>().itemInfo.pickTime;
				if(picking >= other.GetComponent<ItemInfo>().itemInfo.pickTime){
					picking = 0;
					ShowItemInfo(other.gameObject, false);
					AddItem(other.gameObject, other.GetComponent<ItemInfo>().itemInfo.itemID);
				}
			} else {
				pickSlider.gameObject.SetActive(false);
				pickText.color = Color.white;
				picking = 0;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Supply"){
			picking = 0;
			ShowItemInfo(other.gameObject, false);
		}
	}

	public void ShowItemInfo(GameObject collided, bool isEnable){
		collided.transform.Find("ItemPick").gameObject.SetActive(isEnable);
		itemInfoPanel.gameObject.SetActive(isEnable);
	}

	void AddItem(GameObject item, int itemID){
		item.GetComponent<ItemInfo>().isPicked = true;

		switch(itemID){
			case 0:
				ic.wInv[ic.selectedWeapon].wClips++;
				ic.UpdateInvInfo(); 
			break;
			case 1:
				bs.bleedingCount--;
			break;
			case 2:
				ic.grenadeCount++;
			break;
			case 3:
				hts.hunger += 25;
			break;
			case 4:
				hts.thirst += 35;
			break;
		}
	}

	void FixedUpdate()
    {
        rb.MovePosition(rb.position + _inputs * playerSpeed * Time.fixedDeltaTime);
    }

	private float GetSpeed(){
		if(isRunning  && !ss.aim){
			return runSpeed;
		} else if(ss.aim){
			return aimSpeed;
		} else if(isRecovering){
			return recoveringSpeed;
		}
		return normalSpeed;
	}

	IEnumerator Recovering(){
		yield return new WaitForSeconds(6f);
		isRecovering = false;
		stamina = 1f;
		StopAllCoroutines();
	}

	IEnumerator Reload(){
		isReloading = true;
		ic.wInv[ic.selectedWeapon].wClips--;
		ic.UpdateInvInfo();
		yield return new WaitForSeconds(pm.reloadingTime);
		if(pm.ammo == pm.clipsLenght){
			ic.wInv[ic.selectedWeapon].wClips++;
		}
		pm.ammo = pm.clipsLenght;
		ic.UpdateInvInfo();
		isReloading = false;
	}
}
