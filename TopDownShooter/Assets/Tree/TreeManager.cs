using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tree{
public class TreeManager : MonoBehaviour {

	public bool isFalled = false;

	public bool isCutten = false;

	private Transform trunk;

	private Transform barsParent;

	public Transform bar;

	public int status;

	public int barCount;

	public float barSize;

	void Start(){
		trunk = transform.Find("Trunk");
		barsParent = trunk.transform.Find("BarsObject");

		GenerateBars();
	}

	void LateUpdate(){
		if(isCutten){
			trunk.gameObject.SetActive(false);
			barsParent.SetParent(this.transform);
			barsParent.gameObject.SetActive(true);
			isCutten = false;
		}

		if(status <= 0 && !isFalled){
			FallTree();
		}
	}

	private void FallTree(){
		Rigidbody rb = trunk.gameObject.AddComponent<Rigidbody>();
		rb.mass = 50;
		rb.angularDrag = 10f;
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.AddForce(Vector3.forward, ForceMode.Impulse);
		isFalled = true;
	}

	private void GenerateBars(){
		for(int i = 0; i < barCount; i++){
			GameObject bo = Instantiate(bar.gameObject);
			bo.transform.SetParent(barsParent);
			bo.transform.position = new Vector3(barsParent.transform.position.x,barsParent.transform.position.y + (i * barSize),barsParent.transform.position.z);
			bo.transform.localScale = bar.localScale;
			Rigidbody rb = bo.gameObject.AddComponent<Rigidbody>();
			rb.mass = 5;
			rb.angularDrag = 4f;
			rb.isKinematic = false;
			rb.useGravity = true;
			//rb.AddForce(Vector3.forward, ForceMode.Impulse);	
		}
	}

}
}
