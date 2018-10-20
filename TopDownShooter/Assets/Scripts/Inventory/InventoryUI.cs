using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent;
	public Transform fSlotsParent;
	public Transform mainfSlotsParent;
	public Transform mainWeaponPanel;
	public Transform infoParent;
	public Transform interactionParent;

	public Transform weaponParent;

	public Transform secondaryWeapon;

	public Transform actionPanel;
	public Transform interactionActionPanel;

	Inventory inventory;

	InventorySlot[] slots;

	InventorySlot[] fSlots;

	InventorySlot[] mainfSlots;

	InventorySlot[] weaponSlots = new InventorySlot[2];

	public InventorySlot[] iSlots;

	public Transform weight;

	BaseEventData ed;

	InteractionController ic;

	void Start(){
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;
		ic = InteractionController.instance;
		if(ic.iSys != null)
		ic.iSys.onDropChangedCallBack += UpdateInteractionUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		fSlots = fSlotsParent.GetComponentsInChildren<InventorySlot>();
		mainfSlots = mainfSlotsParent.GetComponentsInChildren<InventorySlot>();
		iSlots = interactionParent.GetComponentsInChildren<InventorySlot>();
		weaponSlots = weaponParent.GetComponentsInChildren<InventorySlot>();
	}

	public void UpdateUI(){
// Action Menu
		if(inventory.isRightClick && inventory.selectedSlot <= inventory.items.Count - 1){
			actionPanel.gameObject.SetActive(true);
			actionPanel.transform.position = itemsParent.GetChild(inventory.selectedSlot).transform.position - (Vector3.up * ((Screen.height * 112f) / 1080));
			actionPanel.GetChild(2).gameObject.SetActive(inventory.items[inventory.selectedSlot].item.canWear);
			actionPanel.GetChild(0).gameObject.SetActive(inventory.items[inventory.selectedSlot].item.isWeapon);
		} else {
			actionPanel.gameObject.SetActive(false);
		}

		SetSelectedCell(ed);

		ChangeWeaponPanel();

// Weight Change
		weight.GetChild(0).GetComponent<Text>().text = inventory.weight.ToString("f3") + " kg";
		weight.GetChild(1).GetComponent<Text>().text = inventory.maxWeight + " kg";
		for (int i = 0; i < slots.Length; i++)
		{
			if(i < inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i].item);
				if (inventory.items[i].count > 1){
					slots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
					slots[i].transform.GetChild(2).GetComponent<Text>().enabled = true;
					slots[i].transform.GetChild(2).GetComponent<Text>().text = "x " + inventory.items[i].count;
				} else {
					slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
					slots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
				}
			} else {
				slots[i].ClearItem();
				slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
				slots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
			}
		}

		for(int i = 0; i < 2; i++){
			if(i < inventory.weaponItems.Count)
			{
				weaponSlots[i].AddItem(inventory.weaponItems[i].item);
			} else {
				weaponSlots[i].ClearItem();
			}

			if(inventory.weaponItems[i].item == null){
				weaponSlots[i].ClearItem();
			}
		}


		for(int i = 0; i < 3; i++){
			if(i < inventory.fastItems.Count)
			{
				fSlots[i].AddItem(inventory.fastItems[i].item);
				mainfSlots[i].AddItem(inventory.fastItems[i].item);
				int c = SumItemType(inventory.fastItems[i].item);
				if(c <= 0){
					inventory.RemoveFastSlot(i);
					fSlots[i].ClearItem();
					mainfSlots[i].ClearItem();	
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
					mainfSlots[i].transform.GetChild(3).gameObject.SetActive(false);
					return;
				} else if(c > 1){
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = true;
					mainfSlots[i].transform.GetChild(3).gameObject.SetActive(true);
				} else {
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
					mainfSlots[i].transform.GetChild(3).gameObject.SetActive(false);
				}
				fSlots[i].transform.GetChild(1).GetComponent<Text>().text = "x " + c;
				mainfSlots[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "" + c;
			} else {
				fSlots[i].ClearItem();
				mainfSlots[i].ClearItem();
				fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
				mainfSlots[i].transform.GetChild(3).gameObject.SetActive(false);
			}
		}
	}

	/*public void UpdateWeaponPanel(int ammoCount, Slot selectedSlot){
		// Weapon Panel

	}*/

	public void UpdateInteractionUI(){
		if(ic.clicked && ic.selectedSlot <= ic.iSys.dropItems.Count - 1){
			interactionActionPanel.gameObject.SetActive(true);
			interactionActionPanel.transform.position = interactionParent.GetChild(ic.selectedSlot).transform.position - (Vector3.up * ((Screen.height * 98f) / 1080));
			//interactionActionPanel.GetChild(1).gameObject.SetActive(inventory.items[inventory.selectedSlot].item.canWear);
		} else {
			interactionActionPanel.gameObject.SetActive(false);
		}

		SetInteractionSelectedCell(ed, ic.iSys);

		for (int i = 0; i < iSlots.Length; i++)
		{
			if(i < ic.iSys.dropItems.Count)
			{
				iSlots[i].AddItem(ic.iSys.dropItems[i].item);
				if (ic.iSys.dropItems[i].count > 1){
					iSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
					iSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = true;
					iSlots[i].transform.GetChild(2).GetComponent<Text>().text = "x " + ic.iSys.dropItems[i].count;
				} else {
					iSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
					iSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
				}
			} else {
				iSlots[i].ClearItem();
				iSlots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
				iSlots[i].transform.GetChild(2).GetComponent<Text>().enabled = false;
			}
		}
	}

	private int SumItemType(Item item){
	int count = 0;
		for(int i = 0; i < inventory.items.Count; i++){
			if(inventory.items[i].item == item){
				count+=inventory.items[i].count;
			}
		}
		return count;
	} 

	private void ChangeWeaponPanel(){
		for(int i = 0; i < inventory.weaponItems.Count; i++){
			if(inventory.weaponItems[0].item != null){
				mainWeaponPanel.gameObject.SetActive(true);
				mainWeaponPanel.GetChild(1).GetComponent<Image>().sprite = inventory.weaponItems[0].item.icon;
				mainWeaponPanel.GetChild(2).GetComponent<Text>().text = inventory.weaponItems[0].item.name;
				//mainWeaponPanel.GetChild(3).GetComponent<Text>().text = inventory.weaponItems[0].item.capacity + "";
			} else {
				mainWeaponPanel.gameObject.SetActive(false);
			}

			if(inventory.weaponItems[1].item != null){
				secondaryWeapon.gameObject.SetActive(true);
				secondaryWeapon.GetChild(0).GetComponent<Image>().sprite = inventory.weaponItems[1].item.icon;
				secondaryWeapon.GetChild(1).GetComponent<Text>().text = inventory.weaponItems[1].item.name;
			} else {
				secondaryWeapon.gameObject.SetActive(false);
			}
		}
	}

	public void SetInteractionSelectedCell(BaseEventData ed, InteractionSystem s){
		int cellID = ic.selectedSlot;
		Transform cell = interactionParent.GetChild(cellID);
		for(int i = 0; i < interactionParent.childCount; i++) { interactionParent.GetChild(i).GetComponent<Button>().OnDeselect(ed);}
		cell.GetComponent<Button>().OnSelect(ed);
	}

	public void SetSelectedCell(BaseEventData ed){
		int cellID = inventory.selectedSlot;
		Transform cell = itemsParent.GetChild(cellID);
		for(int i = 0; i < itemsParent.childCount; i++) { itemsParent.GetChild(i).GetComponent<Button>().OnDeselect(ed);}
		cell.GetComponent<Button>().OnSelect(ed);
		ItemInfo(cellID);
	}

	private void ItemInfo(int id){
		if(inventory.selectedSlot >= inventory.items.Count){
			infoParent.gameObject.SetActive(false);
			return;
		}
		infoParent.gameObject.SetActive(true);
		infoParent.GetChild(0).GetComponent<Image>().sprite = inventory.items[id].item.icon;
		infoParent.GetChild(0).GetChild(0).GetComponent<Text>().text = inventory.items[id].item.name;
		infoParent.GetChild(0).GetChild(1).GetComponent<Text>().text = inventory.items[id].item.type.ToString();
		infoParent.GetChild(0).GetChild(1).GetComponent<Text>().color = inventory.items[id].item.itemColor;
		infoParent.GetChild(1).GetChild(1).GetComponent<Text>().text = inventory.items[id].item.description;
		infoParent.GetChild(3).GetChild(0).GetComponent<Text>().text = inventory.items[id].item.weight.ToString("f3") + "\nkg";
	}
}
