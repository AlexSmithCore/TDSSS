using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
	
	new public string name = "New Item";
	public Sprite icon = null; 

	public float weight;

	public int stackSize;

	[TextArea]
	public string description;

	public Color itemColor;

	public string type;

	public GameObject prefab;

	public virtual void Use(){
		Debug.Log("Using " + name);
	}
}
