﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GameController : MonoBehaviour {

	public bool isDead;

	public bool isPause;

	public float distToBase;

	public bool isWeaponPick;

	public Transform player;

	public Camera cam;

	public GameObject deadUI;
	public GameObject gameUI;

	public Sprite[] weaponType;

	public Animator anim;

	public Image cursor;

	public GameObject pauseUI;

	public float numberOfPixelsNorthToNorth;
	public Image compas;
	public GameObject home;

	private GameObject homeUI;
    float rationAngleToPixel;

	public Transform basePoint;

	public Text distToBaseText;

	public GameObject weaponPickPanel;

	public PostProcessingProfile ppp;

	private float blurTrans;

	private float pointToBlur = 3.15f;

	private float maxPointBlur = 3.35f;

	public float playerLastRot;

	DepthOfFieldModel.Settings dofSettings;

	Inventory inventory;

	void Start(){
		cam = FindObjectOfType<Camera>();

		inventory = Inventory.instance;

		weaponPickPanel.SetActive(false);
        rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;

		dofSettings = ppp.depthOfField.settings;

		homeUI = home.transform.GetChild(0).gameObject;
	}

	void Update () {
		if(!isWeaponPick){
			Vector3 mousePos = Input.mousePosition;
			cursor.transform.position = mousePos;
			cursor.transform.eulerAngles += Vector3.forward * Time.deltaTime * 45;
		}

		if(isDead){
			gameUI.SetActive(false);
			deadUI.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.C)){
			inventory.selectedSlot = 0;
			inventory.isRightClick = false;
			inventory.onItemChangedCallBack.Invoke();
			isWeaponPick = !isWeaponPick;
			weaponPickPanel.SetActive(isWeaponPick);

			if(isWeaponPick){
				Time.timeScale = 0.1f;
				pointToBlur = 0.1f;
				distToBase = (player.transform.position - basePoint.transform.position).magnitude;
				Color alphaColor = new Color(1f,1f,1f,distToBase / 75f);
				homeUI.GetComponent<Image>().color = alphaColor;
				homeUI.transform.GetChild(0).GetComponent<Text>().color = alphaColor;
				homeUI.transform.GetChild(1).GetComponent<Text>().color = alphaColor;
				playerLastRot = player.transform.eulerAngles.y;
				compas.transform.rotation = Quaternion.Euler(Vector3.zero);
				Vector3 dir = basePoint.transform.position - player.transform.position;
 				float angle = Mathf.Atan2(dir.x, dir.z)*Mathf.Rad2Deg;
				home.transform.localRotation = Quaternion.Euler(0, 0, player.transform.root.rotation.eulerAngles.y - angle);
				distToBaseText.text = (int)distToBase + " m";
			} else {
				Time.timeScale = 1f;
				pointToBlur = maxPointBlur;
			}

			cam.GetComponent<CameraMovement>().freeze = isWeaponPick;
			player.GetComponent<PlayerControl>().isFreeze = isWeaponPick;
		}

		blurTrans = Mathf.Lerp(dofSettings.focusDistance, pointToBlur, Time.deltaTime * 20f);
		dofSettings.focusDistance = blurTrans;
		ppp.depthOfField.settings = dofSettings;

		Time.timeScale = blurTrans / maxPointBlur;

		compas.color = new Color(1f,1f,1f,1f - Time.timeScale);

		if(Input.GetKeyDown(KeyCode.Escape)){
			isPause = !isPause;

			if(isPause){
				Time.timeScale = 0f;
				pointToBlur = 0f;
			} else {
				Time.timeScale = 1f;
				pointToBlur = maxPointBlur;
			}
		}

		compas.transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(compas.transform.eulerAngles.z,playerLastRot, Time.unscaledDeltaTime * 10f));

		Cursor.visible = isWeaponPick;
		pauseUI.SetActive(isPause);
		cursor.gameObject.SetActive(!isWeaponPick);
	}

	private float CheckAngle(float angle){
		if(player.localEulerAngles.y < 180){
			return angle;
		} else if (player.localEulerAngles.y > 180){
			return -angle;
		}
		return 0;
	}

	public void ExitGame(){
		Application.Quit();
	}
}
