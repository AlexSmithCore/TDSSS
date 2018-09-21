using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class HumanController : MonoBehaviour {

	[Header("Characteristics")]
	public bool isDetected = false;

	public bool isWorking = false;

	[SerializeField]
	private bool checkForWork = false;

	public bool goToStock = false;

	[SerializeField]
	private bool isCuttingsTrees = false;

	public bool isAim;

	public Transform enemyTarget;

	public Transform mainTarget;

	public Transform curTarget;

	private float distToFollow;

	public float Speed = 10f;

	[Header("Field Of View")]
	[Space]
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	private HumanManager hm;
	private NavMeshAgent human;
	private Animator animator;

	public Light shootLight;

	public GameObject PS_shoot;

	
	public float bulletSpeed;

	public float interval;

	float shotCounter;

	public Transform firePoint;

	public BulletController bullet;

	public float distToEnemy;

	public GameObject retreatPoint;

	public GameObject sleevesPoint;
	public GameObject sleeve;

	public float spreadSize;

	void Start(){
		PS_shoot.SetActive(false);
		shootLight.enabled = false;
		hm = GetComponent<HumanManager>();
		animator = GetComponent<Animator>();
		human = GetComponent<NavMeshAgent>();
		mainTarget = GameObject.FindGameObjectWithTag("Player").transform;
		curTarget = mainTarget;
		StartCoroutine ("FindTargetsWithDelay", .2f);
	}

	
	void FixedUpdate(){

		if((int)hm.employment == 1){

//Warrior
			if(isDetected && enemyTarget != null){
				RotateTowards(enemyTarget);
				if ((transform.position - enemyTarget.transform.position).magnitude <= distToEnemy){
					Vector3 retreatPointMathf = transform.position + (transform.position - enemyTarget.transform.position).normalized * distToEnemy;
					retreatPointMathf.y = 0;
					retreatPoint.transform.position = retreatPointMathf;
					curTarget = retreatPoint.transform;
				} else {
					curTarget = mainTarget;
				}
			} else {
				curTarget = mainTarget;
			}

			human.SetDestination(curTarget.transform.position);

			isAim = isDetected;

			if(isAim && enemyTarget != null){
				shotCounter -= Time.deltaTime;
				if(shotCounter <= 0){
					shootLight.enabled = true;
					PS_shoot.SetActive(true);
					Invoke("ShootEffect", .1f);
					shotCounter = interval - Random.Range(0f,0.3f);
					float xSpread = Random.Range(-1, 1);
					float ySpread = Random.Range(-1, 1);
					//normalize the spread vector to keep it conical
					Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * spreadSize;
					Quaternion rotation = Quaternion.Euler(spread) * transform.rotation;
					BulletController newBullet = Instantiate(bullet, firePoint.position, rotation) as BulletController;
					newBullet.speed = bulletSpeed;
					GameObject newSleeve = Instantiate(sleeve, sleevesPoint.transform.position, Random.rotation);
					newSleeve.GetComponent<Rigidbody>().AddForce(transform.right * 64);
				}
			} else {
				shotCounter = 0;
			}

			} else if((int)hm.employment == 2) {

// WoodChopper

				if(!isWorking){
					if(!checkForWork){
						StartCoroutine(WorkCheckDelay(1));
					}
				} else {
					human.SetDestination(curTarget.transform.position);
					if((transform.position - curTarget.transform.position).magnitude <= 2.5f){
						RotateTowards(curTarget);
						if(!isCuttingsTrees){
							if(!goToStock){
								StartCoroutine(CuttingsTrees());
							}
						}
					}

					if((transform.position - curTarget.transform.position).magnitude <= 2.5f && goToStock){
						isWorking = false;
						goToStock = false;
						checkForWork = false;
						curTarget.GetComponent<StockManager>().AddToStock(this.transform,5);
					}
				}
			} else {

//Nothing

		}

		// Animator
		if (human.velocity.magnitude >= Speed || human.velocity.magnitude < 0){
			animator.SetFloat("Move", 1f);
		}
		else{
			animator.SetFloat("Move", human.velocity.magnitude/Speed);
		}

		//Debug.Log(transform.rotation.eulerAngles);
		animator.SetFloat("ForBack", human.velocity.normalized.z * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		+ human.velocity.normalized.x * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));
		animator.SetFloat("LeftRight", human.velocity.normalized.x * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
		-  human.velocity.normalized.z * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180));
	}

	void ShootEffect(){
		PS_shoot.SetActive(false);
		shootLight.enabled = false;
	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	IEnumerator WorkCheckDelay(int item){
		checkForWork = true;
		yield return new WaitForSeconds(2f);
		isWorking = hm.FindItem(item);
		if(!isWorking){
			StartCoroutine(WorkCheckDelay(item));
		} else {
			curTarget = FindTree();
			StopAllCoroutines();
		}
	}

	IEnumerator CuttingsTrees(){
// Start cutting tree
		isCuttingsTrees = true;
		yield return new WaitForSeconds(2f);
// Wood added
		hm.AddItem(1,1);
		goToStock = hm.IsItemsWeHave(1,5);
		if(!goToStock){
			isCuttingsTrees = false;
		} else {
			curTarget = FindNearestStock("WoodStock");
			isCuttingsTrees = false;
		}
	}

	Transform FindTree(){
		GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
		int m = 0;
 		for (int i = 1; i < trees.Length; i++){
  			if (((transform.position - trees[i].transform.position).magnitude < (transform.position - trees[m].transform.position).magnitude)){
  				m = i;
			}
		}
// Tree Founded!
	 	return trees[m].transform;
	}

	public void FindVisibleTargets() {
		if((int)hm.employment == 1){
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
		int count = 0;
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					count++;
					visibleTargets.Add (target);
					enemyTarget = GetClosestGO(transform.position,targetsInViewRadius);
					isDetected = !enemyTarget.GetComponent<EnemyController>().isDead;
				}
			}
		}

		if(count == 0){
			isDetected = false;
		}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	Transform GetClosestGO(Vector3 _pos, Collider[] _targets) {
 	int m = 0;
 		for (int i = 1; i < _targets.Length; i++){
  			if (((_pos - _targets[i].transform.position).magnitude < (_pos-_targets[m].transform.position).magnitude)){
  				m = i;
			}
		}
	 	return _targets[m].transform;
	}

	private void RotateTowards (Transform target) {
		if(enemyTarget != null){
            Vector3 direction = (target.position - firePoint.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 12f);
		}
    }

	Transform FindNearestStock(string stockTag){
		GameObject[] stocks = GameObject.FindGameObjectsWithTag(stockTag);
		int m = 0;
 		for (int i = 1; i < stocks.Length; i++){
  			if (((transform.position - stocks[i].transform.position).magnitude < (transform.position - stocks[m].transform.position).magnitude)){
  				m = i;
			}
		}
	 	return stocks[m].transform;
	}

}
