using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour {

	public bool isDetected = false;

	private PlayerController target;

	private NavMeshAgent human;

	public AllAgentsManager am;

	void Start(){
		am = GameObject.Find("HumanAndMosterController").GetComponent<AllAgentsManager>();

		human = GetComponent<NavMeshAgent>();
		target = FindObjectOfType<PlayerController>();

		for(int i = 0; i < am.alliesSize; i++){
			if(am.allies[i] == null){
				am.allies[i] = this.gameObject;
				break;
			}
		}
	}

	void FixedUpdate(){
		human.SetDestination(target.transform.position);
	} 

	void OnCollisionEnter(Collision other){
		Debug.Log(other.collider.tag);
		if(other.collider.tag == "Enemy" && !isDetected){
			isDetected = true;
		}
	}

	void OnCollisionExit(Collision other){
		print(other.collider.tag);
		if(other.collider.tag == "Enemy"){
			isDetected = false;
		}
	}
}
