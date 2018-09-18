using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour {

	public string hName;

	public enum Employment{
		none = 0,
		warrior = 1,
		woodcutter = 2,
	}

	public Employment employment;

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
