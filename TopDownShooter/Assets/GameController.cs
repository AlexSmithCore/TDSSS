using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool isDead;

	public bool isPause;

	public GameObject deadUI;
	public GameObject gameUI;

	public Sprite[] weaponType;

	public Animator anim;

	public Image cursor;

	public GameObject pauseUI;

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

		if(Input.GetKeyDown(KeyCode.Escape)){ isPause = !isPause; }
		Cursor.visible = isPause;
		pauseUI.SetActive(isPause);
		cursor.gameObject.SetActive(!isPause);
	}

	public void ExitGame(){
		Application.Quit();
	}
}
