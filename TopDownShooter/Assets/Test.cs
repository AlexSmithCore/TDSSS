using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void Update(){
		transform.RotateAround(Vector3.zero, Vector3.forward, Time.deltaTime * 150f);
	}
}
