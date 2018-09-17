using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyRotation : MonoBehaviour {

	private void FixedUpdate()
	{
		transform.eulerAngles += Vector3.up * 24 * Time.deltaTime;	
	}
}
