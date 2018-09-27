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

	void Update(){
		unityNameText.transform.position = cam.WorldToScreenPoint(transform.position + (Vector3.up * 3f));
	}

	public bool FindItem(int itemID){
		for(int i = 0; i < hInv.Length; i++){
			if(hInv[i].itemID == itemID){
				return false;
			}
		}
		return true;
	}

	public void AddItem(int itemID, int itemCount){
		int m = 0;
		for(int i = 0; i < hInv.Length; i++){
			if(hInv[m].itemID == 0){
				m = i;
			} else if(hInv[i].itemID == itemID){
				m = i;
				break;
			}
		}

		hInv[m].itemID = itemID;
		hInv[m].itemCount += itemCount;
	}

	public bool IsItemsWeHave(int itemID, int itemCount){
		for(int i = 0; i < hInv.Length; i++){
			if(hInv[i].itemID == itemID && hInv[i].itemCount >= itemCount){
				return true;
			}
		}
		return false;
	}

	public void DeleteItems(int itemID, int itemCount){
		int m = 0;
		for(int i = 0; i < hInv.Length; i++){
			if(hInv[i].itemID == itemID){
				m = i;
				break;
			}
		}

		hInv[m].itemCount -= itemCount;
		UpdateInventory();
	}

	private void UpdateInventory(){
		for(int i = 0; i < hInv.Length; i++){
			if(hInv[i].itemID > 0 && hInv[i].itemCount <= 0){
				hInv[i].itemID = 0;
				hInv[i].itemCount = 0;
				hInv[i].itemName = null;
			}
		}
	}

	public HInventory[] hInv;

	[System.Serializable]
	public class HInventory{
		public string itemName;
		public int itemID;
		public int itemCount;
	}	
}
