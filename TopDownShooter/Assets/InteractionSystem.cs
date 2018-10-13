using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {
	public bool interacted;

	public GameObject materialsBody;

	[SerializeField]
	private Material _material;

	public Color _interactColor;
	public Color _disableColor;

	void Start(){
		_material = materialsBody.GetComponent<Renderer>().material;
	}

	public void Interact(){
		if(interacted){
			_material.SetColor("_OutlineColor", _interactColor);
		} else { 
			_material.SetColor("_OutlineColor", _disableColor);
		}
	}
}
