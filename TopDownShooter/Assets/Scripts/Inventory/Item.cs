using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public bool stackable;
	new public string name = "New Item";
	public Sprite icon = null; 

	public float weight;

	public virtual void Use(){
		Debug.Log("Using " + name);
	}
}
