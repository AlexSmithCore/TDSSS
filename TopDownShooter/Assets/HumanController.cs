using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour {

	public bool isDetected = false;

	private PlayerController target;

	private NavMeshAgent human;

	public AllAgentsManager am;

    public float FollowPlayer = 30f;

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

    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Enemy" && !isDetected)
        {
            isDetected = true;
            //target = other.tranform.position;
            Debug.Log(other.tag);
        }
    }

    void OnTriggerExit(Collider other){
		if(other.tag == "Enemy"){
			isDetected = false;
            //target = FindObjectOfType<PlayerController>();
        }
	}
}
