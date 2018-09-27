using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public bool isBusy;

	public Image icon;
	public Text itemCount;
	Item item;

	public void AddItem(Item newItem){
		item = newItem;
		icon.sprite = item.icon;
		icon.enabled = true;
	}

	public void ClearItem(){
		item = null;
		icon.sprite = null;
		icon.enabled = false;
	}

	public void UseItem(){
		if(item != null){
			item.Use();
		}
	}

}
