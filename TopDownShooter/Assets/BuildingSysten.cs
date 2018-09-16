using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSysten : MonoBehaviour {
	// ID этажа, на котором находится игрок
	public int currentFloor = -1;
	// Количество этажей во всём здании
	public int floorsCount;

	// Массив с ссылками на объекты этажей
	public GameObject[] floors;

	// Допустимая высота, чтобы переместить на другой этаж
	public float[] floorHeights;

	// Сделал эту переменную для ограничения выполнения действий в методе, может она не нужна
	private float activeFloor = -1;

	void Start(){
		// Записываем количество этажей и сами объекты дочерних объектов
		floorsCount = this.transform.childCount;
		floors = new GameObject[floorsCount];

		for(int i = 0; i < floorsCount; i++){
			floors[i] = this.transform.GetChild(i).gameObject;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		// Проверяю, если игрок находится на определённой высоте, то его конкретный этаж меняется на то, что записано в массиве floorHeights
		if(other.tag == "Player"){
			float objHeight = other.transform.position.y;
			for(int h = floorsCount - 1; h >= 0; h--){
				if(objHeight >= floorHeights[h])
				{
					currentFloor = h;
					break;
				}
			}
			// Как раз таки ограничитель действий
			if(activeFloor != currentFloor){
				// Запуск метода активации этажей
				ActivateFloor(currentFloor);
			}
		}
	}

	private void ActivateFloor(int floorsToActive){
		for(int f = 0; f <= floorsCount - 1; f++){
			// Назначаю объект для того, чтобы было проще соображать / работать с кодом
			GameObject floor = this.transform.GetChild(f).gameObject;
			// Данная система проверяет, сколько объектов ( снизу - вверх ) нужно активировать, либо убрать со сцены
				if(f <= floorsToActive){
					//Invoke("SetMaterialOpaque", 1.0f);
					SetMaterialOpaque();
					//floor.SetActive(true);
				} else {
					SetMaterialTransparent(floor);
					//floor.SetActive(false);
				}
			}
		// После сие действия ставим ограничитель 
		activeFloor = floorsToActive;
	}

	/*private void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			for(int i = 0; i <= floors.Length; i++){
				iTween.FadeTo(floors[0],0,1);
				SetMaterialTransparent();
			}
			//ActivateFloor(currentFloor);
		}
	}*/

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player"){
			ActivateFloor(-1);
			//iTween.FadeTo(floors[currentFloor],1,1);

		}
	}

	private void SetMaterialTransparent(GameObject floor){
		for(int f = floorsCount - 1; f > currentFloor; f--){
			for(int ch = 0; ch < floors[f].transform.childCount; ch++){
				iTween.FadeTo(floors[f].transform.GetChild(ch).gameObject,0,0.3f);
				foreach(Material m in floors[f].transform.GetChild(ch).GetComponent<Renderer>().materials){
					m.SetFloat("_Mode", 3);
					m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					m.SetInt("_ZWrite", 0);
					m.DisableKeyword("_ALPHATEST_ON");
					m.EnableKeyword("_ALPHABLEND_ON");
					m.DisableKeyword("ALPHAPREMULTIPLY_ON");
					m.renderQueue = 3000;
				}
			}
		}
	}

	private void SetMaterialOpaque(){
		for(int f = 0; f <= currentFloor; f++){
			for(int ch = 0; ch < floors[f].transform.childCount; ch++){
				StartCoroutine(MaterialOpaqueInvoke(f,ch));
			}
		}
	}

	IEnumerator MaterialOpaqueInvoke(int f, int ch){
		iTween.FadeTo(floors[f].transform.GetChild(ch).gameObject,1,0.3f);
		yield return new WaitForSeconds(0.3f);
		foreach(Material m in floors[f].transform.GetChild(ch).GetComponent<Renderer>().materials){
			m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			m.SetInt("_ZWrite", 1);
			m.DisableKeyword("_ALPHATEST_ON");
			m.DisableKeyword("_ALPHABLEND_ON");
			m.DisableKeyword("ALPHAPREMULTIPLY_ON");
			m.renderQueue = -1;
		}
	}
}
