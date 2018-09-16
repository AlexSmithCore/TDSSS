using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public int sceneToGo;

	public Slider slider;

	void Start(){
		StartCoroutine(LoadAsynchronously(sceneToGo));
	}

	IEnumerator LoadAsynchronously(int sceneID){
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

		while(!operation.isDone){
			float progress = Mathf.Clamp01(operation.progress /.9f);
			slider.value = progress;
			yield return null;
		}

	}
}
