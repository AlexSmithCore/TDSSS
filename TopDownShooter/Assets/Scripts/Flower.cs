using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player"){
			animator.Play("Open", 0);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player"){
			animator.Play("Close", 0);
		}
	}

}
