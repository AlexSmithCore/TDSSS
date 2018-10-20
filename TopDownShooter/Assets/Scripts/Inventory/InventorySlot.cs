using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler {

	public bool isFastSlot;
	public bool isInteractionSlot;
	public bool isWeaponSlot;
	public Image icon;
	Item item;

	Inventory inventory;

	InteractionController ic;

	void Start(){
		inventory = Inventory.instance;
		ic = InteractionController.instance;
	}

	public void AddItem(Item newItem){
		if(newItem != null){
			item = newItem;
			icon.sprite = item.icon;
			icon.enabled = true;
			if(isFastSlot){
				GetComponent<Image>().color = newItem.itemColor;
			}
		}
	}

	public void ClearItem(){
		item = null;
		icon.sprite = null;
		icon.enabled = false;
		if(isFastSlot){
			GetComponent<Image>().color = new Color(255,255,255,0.5f);
		}
	}

	/*public void UseItem(){
		if(item != null){
			item.Use();
		}
	}*/

	public void OnPointerDown(PointerEventData eventData)
    {
		int slot = int.Parse(this.name);
		if(isFastSlot){
			inventory.isRightClick = false;
			inventory.RemoveFastSlot(slot);
			// Left click!
			inventory.onItemChangedCallBack.Invoke();
			return;
		}

		if(isWeaponSlot){
			inventory.isRightClick = false;
			inventory.RemoveWeapon(slot);
			inventory.onItemChangedCallBack.Invoke();
			return;
		}

		if(isInteractionSlot){
			ic.selectedSlot = slot;
			ic.clicked = true;
			FindObjectOfType<InventoryUI>().UpdateInteractionUI();
			return;
		}

		inventory.selectedSlot = slot;
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
