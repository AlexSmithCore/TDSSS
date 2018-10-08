using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GameController : MonoBehaviour {

	public bool isDead;

	public bool isPause;

	public float distToBase;

	public bool isWeaponPick;

	public Transform player;

	public CameraMovement cam;
	public Image cursor;
	public Image compas;
	public GameObject home;

	private GameObject homeUI;
	public Transform basePoint;

	public Text distToBaseText;

	public GameObject weaponPickPanel;

	public PostProcessingProfile ppp;

	private float blurTrans;

	private float pointToBlur = 3.15f;

	private float maxPointBlur = 3.35f;

	public float playerLastRot;

	public GameObject usingUI;

	private float progessUsing;

	DepthOfFieldModel.Settings dofSettings;

	Inventory inventory;

	public Transform unit;

	void Start(){
		cam = FindObjectOfType<CameraMovement>();

		inventory = Inventory.instance;

		weaponPickPanel.SetActive(false);

		dofSettings = ppp.depthOfField.settings;

		homeUI = home.transform.GetChild(0).gameObject;
		Cursor.visible = isWeaponPick;
	}

	void Update () {
		if(!isWeaponPick){
			Vector3 mousePos = Input.mousePosition;
			cursor.transform.position = mousePos;
			cursor.transform.eulerAngles += Vector3.forward * Time.deltaTime * 45;
		}

		if(Input.GetKey(KeyCode.Alpha1)){
			if(Input.anyKeyDown){
				progessUsing = 0;
				return;
			}
			if(inventory.fastItems.Count > 0){
				usingUI.SetActive(true);
				progessUsing+= Time.deltaTime;
				usingUI.transform.GetChild(0).GetComponent<Image>().fillAmount = progessUsing / inventory.fastItems[0].item.timeToUse;
				usingUI.transform.GetChild(1).GetComponent<Text>().text = (int)((inventory.fastItems[0].item.timeToUse - progessUsing)+1) + " sec";
				if(progessUsing >= inventory.fastItems[0].item.timeToUse){
					progessUsing=0;
					inventory.fastItems[0].item.Use();
					return;
				}
			} else {
				usingUI.SetActive(false);
				progessUsing = 0;
				return;
			}
		} else {
			usingUI.SetActive(false);
			progessUsing = 0;
		}

		/*if(Input.GetKeyDown(KeyCode.F)){
			cam.isInteraction = !cam.isInteraction;

			if(cam.isInteraction){
				cam.target = unit;
				cam.interactionPoint = unit.transform.GetChild(1);
				unit.GetComponent<HumanController>().isInteracting = true;
			} else {
				cam.target = player;
				unit.GetComponent<HumanController>().isInteracting = false;
			}
		}*/

		if(Input.GetKeyDown(KeyCode.C)){
			inventory.selectedSlot = 0;
			inventory.isRightClick = false;
			inventory.onItemChangedCallBack.Invoke();
			isWeaponPick = !isWeaponPick;
			weaponPickPanel.SetActive(isWeaponPick);
			Cursor.visible = isWeaponPick;
			cursor.gameObject.SetActive(!isWeaponPick);

			//player.GetComponent<TemperaturesController>().ChangeTemperature();

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
		
		if(isWeaponPick)
		compas.transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(compas.transform.eulerAngles.z,playerLastRot, Time.unscaledDeltaTime * 10f));
	
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
