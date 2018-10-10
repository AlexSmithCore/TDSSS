using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSystem : MonoBehaviour {

	#region Variables
	public Transform p_Background;
	public float p_Speed;
	public float p_ParallaxClamp;
	public float p_Smooth;

	Vector3 velocity;

	#endregion

	void Awake(){
		DontDestroyOnLoad(gameObject);
	}

	void Update(){
		float x = Input.GetAxisRaw("Mouse X") * p_Speed;
		Vector3 parallaxPosition = Vector3.ClampMagnitude(Vector3.right * x * Time.deltaTime, p_ParallaxClamp);
		Vector3 smoothParallax = Vector3.SmoothDamp(p_Background.position, parallaxPosition, ref velocity, p_Smooth);
		p_Background.position = smoothParallax;
	}

	public IEnumerator Play(){
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(2);
	}
}
