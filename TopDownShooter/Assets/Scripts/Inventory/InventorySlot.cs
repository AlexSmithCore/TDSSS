using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler {

	public bool isFastSlot;
	public Image icon;
	Item item;

	Inventory inventory;

	void Start(){
		inventory = Inventory.instance;
	}

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

	public void OnPointerDown(PointerEventData eventData)
    {
		if(!isFastSlot){
		inventory.selectedSlot = int.Parse(this.name);
			if(Input.GetMouseButtonDown(1)){
				//Right click!
				inventory.isRightClick = true;
				inventory.onItemChangedCallBack.Invoke();
				return;
			}
			// Left click!
			inventory.isRightClick = false;
			inventory.onItemChangedCallBack.Invoke();
		}
    }
}
