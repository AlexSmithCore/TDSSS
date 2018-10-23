using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanManager : MonoBehaviour {

	public string hName;

	public string[] posibleNames;

	public Transform unitNames;
	public GameObject unitNameUI;

	public GameObject unityNameText;

	public Camera cam;

	public enum Employment{
		none = 0,
		warrior = 1,
		woodcutter = 2,
	}

	public Employment employment;

	void Start(){
		int rand = Random.Range(0, posibleNames.Length);
		hName = posibleNames[rand];
		GameObject obj = Instantiate(unitNameUI, Vector3.zero, Quaternion.identity) as GameObject;
		obj.GetComponent<Text>().text = hName;
		obj.transform.SetParent(unitNames);
		unityNameText = obj;
	}

	void FixedUpdate(){
		unityNameText.transform.position = cam.WorldToScreenPoint(transform.position + (Vector3.up * 3f));
	}
}
