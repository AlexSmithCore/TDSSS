using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Play", 5f);
	}

	private void Play(){
		Destroy(FindObjectOfType<Camera>().gameObject);
		Application.LoadLevel(2);
	}
	

}
