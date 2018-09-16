using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampFireSystem : MonoBehaviour {

	public int capacity;

	public float waterCount;

	public Image waterCounter;
	public GameObject waterDropUI;

	public Camera cam;

	private Text waterCountUI;

	void Start(){
		waterDropUI.transform.Find("CapacityCount").GetComponent<Text>().text = capacity + " ml";
		waterCountUI = waterDropUI.transform.Find("WaterCount").GetComponent<Text>();
	}

	void Update(){
		waterCount += 0.5f * Time.deltaTime;

		if(waterDropUI.activeInHierarchy == true){
			waterDropUI.transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z));
			waterCounter.fillAmount = waterCount / capacity;
			waterCountUI.text = (int)waterCount + " ml";
		}
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Player"){
			float dist = Vector3.Distance(transform.position, other.transform.position);
			if(dist <= 3.5f){
				waterDropUI.SetActive(true);
			} else {
				waterDropUI.SetActive(false);
			}
		}
	}
}
