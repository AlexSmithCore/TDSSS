using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour {

	public bool isDetected = false;

	public Transform mainTarget;
	[SerializeField]
	private Transform target;

	[SerializeField]
	private float distToFollow;

	[SerializeField]
	private int keepDist;

	private float distToMain;

	private float distToEnemy;

	public GameObject[] enemiesList;

	private NavMeshAgent human;

	public AllAgentsManager am;

	private float bestDist;

	private Animator animator;
	public float Speed = 10f;

	void Start(){
		bestDist = keepDist;
		am = GameObject.Find("HumanAndMosterController").GetComponent<AllAgentsManager>();

		animator = GetComponent<Animator>();
		human = GetComponent<NavMeshAgent>();
		mainTarget = GameObject.FindGameObjectWithTag("Player").transform;
		target = mainTarget;
		for(int i = 0; i < am.alliesSize; i++){
			if(am.allies[i] == null){
				am.allies[i] = this.gameObject;
				break;
			}
		}
	}

	
	void FixedUpdate(){
		distToMain = Vector3.Distance(this.transform.position, mainTarget.position);

		if(isDetected){
			distToEnemy = Vector3.Distance(transform.position, target.transform.position);
		} 

		if(distToMain >= distToFollow || !isDetected){
			human.SetDestination(mainTarget.transform.position);
		} else {
			if(distToEnemy <= keepDist){
				human.SetDestination(mainTarget.transform.position);
			}
		}

		if(isDetected){
			this.transform.LookAt(target.transform.position);
		}

		if (human.velocity.magnitude >= Speed || human.velocity.magnitude < 0){
			animator.SetFloat("Move", 1f);
		}
		else{
			animator.SetFloat("Move", human.velocity.magnitude/Speed);
		}
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Enemy"){
			float dist = Vector3.Distance(transform.position, other.transform.position);
			if(dist <= bestDist){
				bestDist = dist;
				target = other.transform;
				isDetected = true;
			}
			float testDist = Vector3.Distance(transform.position, target.position);
			if(bestDist < testDist){
				bestDist = keepDist;
				isDetected = false;
			}
			//Debug.Log(bestDist);
		}
	}


    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
			if(!isDetected){
            	isDetected = true;
            	target = other.transform;
			}
			EnemyCompetition(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other){
		if(other.tag == "Enemy"){
			isDetected = false;
           	//target = mainTarget;
			bestDist = keepDist;
			//EnemyCompetition(other.gameObject);
        }
	}*/

	void EnemyCompetition(GameObject target){
		int test = 0;
		for(int e = 0; e < enemiesList.Length; e++){
			if(enemiesList[e] == target){
				test = e;
				target = CheckFeatures(test);
				break;
			} else {
				if(enemiesList[e] != target && enemiesList[e] == null){
					enemiesList[e] = target;
					test = e;
					target = CheckFeatures(test);
					break;
				}
			}
		}
	}

	private GameObject CheckFeatures(int test){
		float bestDist = keepDist;
		int bestTarget = 0;
		for(int d = 0; d <= test; d++){
			float dist = Vector3.Distance(transform.position, enemiesList[d].transform.position);
			if(dist <= bestDist){
				bestDist = dist;
				bestTarget = d;
			}
		}

		return enemiesList[bestTarget];
	}
}
