using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent;
	public Transform fSlotsParent;
	public Transform mainfSlotsParent;
	public Transform infoParent;

	public Transform actionPanel;

	Inventory inventory;

	InventorySlot[] slots;

	InventorySlot[] fSlots;

	InventorySlot[] mainfSlots;

	public Transform weight;

	BaseEventData ed;

	void Start(){
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		fSlots = fSlotsParent.GetComponentsInChildren<InventorySlot>();
		mainfSlots = mainfSlotsParent.GetComponentsInChildren<InventorySlot>();
	}

	public void UpdateUI(){
// Action Menu
		if(inventory.isRightClick && inventory.selectedSlot <= inventory.items.Count - 1){
			actionPanel.gameObject.SetActive(true);
			actionPanel.transform.position = itemsParent.GetChild(inventory.selectedSlot).transform.position - (Vector3.up * ((Screen.height * 112f) / 1080));
			actionPanel.GetChild(1).gameObject.SetActive(inventory.items[inventory.selectedSlot].item.canWear);
		} else {
			actionPanel.gameObject.SetActive(false);
		}

		SetSelectedCell(ed);

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

		for(int i = 1; i < 4; i++){
			if(i <= inventory.fastItems.Count)
			{
				fSlots[i].AddItem(inventory.fastItems[i-1].item);
				mainfSlots[i-1].AddItem(inventory.fastItems[i-1].item);
				int c = SumItemType(inventory.fastItems[i-1].item);
				if(c <= 0){
					inventory.RemoveFastSlot(i-1);
					fSlots[i].ClearItem();
					mainfSlots[i-1].ClearItem();	
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
					mainfSlots[i-1].transform.GetChild(3).gameObject.SetActive(false);
					return;
				} else if(c > 1){
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = true;
					mainfSlots[i-1].transform.GetChild(3).gameObject.SetActive(true);
				} else {
					fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
					mainfSlots[i-1].transform.GetChild(3).gameObject.SetActive(false);
				}
				fSlots[i].transform.GetChild(1).GetComponent<Text>().text = "x " + c;
				mainfSlots[i-1].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "" + c;
			} else {
				fSlots[i].ClearItem();
				mainfSlots[i-1].ClearItem();
				fSlots[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
				mainfSlots[i-1].transform.GetChild(3).gameObject.SetActive(false);
			}
		}
	}

	/*public void UpdateWeaponPanel(int ammoCount, Slot selectedSlot){
		// Weapon Panel

	}*/

	private int SumItemType(Item item){
	int count = 0;
		for(int i = 0; i < inventory.items.Count; i++){
			if(inventory.items[i].item == item){
				count+=inventory.items[i].count;
			}
		}
		return count;
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
