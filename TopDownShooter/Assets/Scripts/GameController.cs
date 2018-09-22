using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool isDead;

	public bool isPause;

	public Transform player;

	public Camera cam;

	public GameObject deadUI;
	public GameObject gameUI;

	public Sprite[] weaponType;

	public Animator anim;

	public Image cursor;

	public GameObject pauseUI;

	public float numberOfPixelsNorthToNorth;
	public Transform compas;
	Vector3 startPosition;
    float rationAngleToPixel;

	void Start(){
		cam = FindObjectOfType<Camera>();

		startPosition = compas.transform.position;
        rationAngleToPixel = numberOfPixelsNorthToNorth / 360f;
	}

	void Update () {
		if(!isPause){
			Vector3 mousePos = Input.mousePosition;
			cursor.transform.position = mousePos;
			cursor.transform.eulerAngles += Vector3.forward * Time.deltaTime * 45;
		}

		if(isDead){
			gameUI.SetActive(false);
			deadUI.SetActive(true);
			anim.SetTrigger("FadeOut");
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			isPause = !isPause;

			if(isPause){
				Time.timeScale = 0f;
			} else {
				Time.timeScale = 1f;
			}
		}

		Vector3 perp = Vector3.Cross(Vector3.forward, player.transform.forward);
        float dir = Vector3.Dot(perp, Vector3.up);
        compas.transform.position = startPosition + (new Vector3(Vector3.Angle(player.transform.forward, Vector3.forward) * Mathf.Sign(dir) * rationAngleToPixel, 0, 0));

		Cursor.visible = isPause;
		pauseUI.SetActive(isPause);
		cursor.gameObject.SetActive(!isPause);
	}

	public void ExitGame(){
		Application.Quit();
	}
}
