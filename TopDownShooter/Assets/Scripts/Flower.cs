using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

	private Animator animator;

	public Light l;

	private float pointToLight;

	public MeshRenderer r;

	public GameObject ps_lights;

	void Start () {
		animator = GetComponent<Animator>();
		pointToLight = 0;
		ps_lights.SetActive(false);
	}

	void Update(){
		float trans = Mathf.Lerp(l.intensity, pointToLight, Time.deltaTime * 0.5f);
		l.intensity = trans;
		r.materials[0].SetColor("_EmissionColor", new Color(0.5105881f, trans, 0f, 1f)); 
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player"){
			animator.Play("Open", 0);
			pointToLight = 1.4f;
			ps_lights.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player"){
			animator.Play("Close", 0);
			pointToLight = 0;
			ps_lights.SetActive(false);
		}
	}

}
