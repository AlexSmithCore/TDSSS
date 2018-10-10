using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

	public bool gameStart;

	public Transform startPoint;

	public Animator fadeIn;

	public Camera cam;

	public GameObject menuUI;

	public GameObject exit_UI;

	public GameObject options_UI;

	public int selectedMenu;

	public void CloseOptionsUI(){
		options_UI.SetActive(false);
		exit_UI.SetActive(false);
	}

	public void OpenOptionsUI(){
		options_UI.SetActive(!options_UI.activeInHierarchy);
		exit_UI.SetActive(false);
	}

	public void CloseExitUI(){
		exit_UI.SetActive(false);
		options_UI.SetActive(false);
	}

	public void OpenExitUI(){
		exit_UI.SetActive(!exit_UI.activeInHierarchy);
		options_UI.SetActive(false);
	}

	public void ExitGame(){
		Application.Quit();
	}

	public void NewGame(){
		menuUI.SetActive(false);
		cam.GetComponent<ParallaxSystem>().enabled = false;
		fadeIn.SetTrigger("FadeIn");
		gameStart = true;
	}

	void Update(){
		if(!gameStart){
			return;
		}

		if(cam.orthographicSize > 0.1){
			cam.orthographicSize -= Time.deltaTime * 10f;
			Vector3 smoothStart = Vector3.Slerp(cam.transform.position, startPoint.position, .75f * Time.deltaTime);
			smoothStart.z = -10;
			cam.transform.position = smoothStart;
		} else {
			StartCoroutine(cam.GetComponent<ParallaxSystem>().Play());
			Application.LoadLevel(1);
			cam.orthographicSize = 5;

		}
	}

	void Play(){

	}
}
