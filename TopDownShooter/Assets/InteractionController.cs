using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour {

	#region  Singleton
	public static InteractionController instance;

	void Awake(){
		instance = this;
	}

	#endregion

	public int selectedSlot;

	public bool clicked;

	public bool picking;

	public InteractionSystem iSys;

	float timer;

	int countToPick;

	Slider interactionSlider;

	void Update(){
		if(picking && countToPick > 0){
			timer-=Time.deltaTime;
			interactionSlider.value =  1 - (timer / iSys.dropItems[selectedSlot].item.timeToPick);
			if(timer <= 0){
				AddToInventory(iSys.dropItems[selectedSlot].item);
				countToPick--;
				if(countToPick > 0){
					timer = iSys.dropItems[selectedSlot].item.timeToPick;
					interactionSlider.gameObject.SetActive(true);
				}
			}
		}
	}

	
	public void Take(){
		clicked = false;
		countToPick = 1;
		picking = true;
		interactionSlider = FindObjectOfType<InventoryUI>().iSlots[selectedSlot].transform.GetChild(3).GetComponent<Slider>();
		interactionSlider.gameObject.SetActive(true);
		timer = iSys.dropItems[selectedSlot].item.timeToPick;
		FindObjectOfType<InventoryUI>().UpdateInteractionUI();
	}

	public void TakeAll(){
		clicked = false;
		countToPick = iSys.dropItems[selectedSlot].count;
		picking = true;
		interactionSlider = FindObjectOfType<InventoryUI>().iSlots[selectedSlot].transform.GetChild(3).GetComponent<Slider>();
		interactionSlider.gameObject.SetActive(true);
		timer = iSys.dropItems[selectedSlot].item.timeToPick;
		FindObjectOfType<InventoryUI>().UpdateInteractionUI();
	}

	private void AddToInventory(Item item){
		interactionSlider.gameObject.SetActive(false);
		if(countToPick < 0){
			picking = false;
		}
		bool wasPickedUp = Inventory.instance.Add(item, 1);
		iSys.weight -= iSys.dropItems[selectedSlot].item.weight;

		if(iSys.dropItems[selectedSlot].count > 1){
			iSys.dropItems[selectedSlot].count--;
		} else {
			iSys.dropItems.Remove(iSys.dropItems[selectedSlot]);
		}
		FindObjectOfType<InventoryUI>().UpdateInteractionUI();
	}

}
