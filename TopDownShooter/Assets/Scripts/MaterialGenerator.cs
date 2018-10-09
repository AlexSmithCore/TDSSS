using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGenerator : MonoBehaviour {

	public Material material; 

	public float delta = 1f;

	private int width=1024;
	private int height=1024;
	//private Color32[] cols;
	private Color[] pix;
	//private Texture2D texture;
	private Texture2D noiseTex;
	
	void Start () {
		//texture = new Texture2D(width, height);
		//cols = new Color32[width*height];
		noiseTex = new Texture2D(width, height);
        pix = new Color[noiseTex.width * noiseTex.height];

		/*cols[0] = new Color(Random.Range(0,255), Random.Range(0,255), Random.Range(0,255), Random.Range(0,255));
		for (int i = 1; i < cols.Length; i++){
			if (i > width){
				cols[i] = new Color((cols[i-1].r + cols[i-width].r)/2 + Random.Range(-delta, delta), (cols[i-1].g + cols[i-width].g)/2 + Random.Range(-delta, delta), (cols[i-1].b + cols[i-width].b)/2 + Random.Range(-delta, delta), (cols[i-1].a + cols[i-width].a)/2 + Random.Range(-delta, delta));
			}
			else
			{
				cols[i] = new Color(cols[i-1].r + Random.Range(-delta, delta), cols[i-1].g + Random.Range(-delta, delta), cols[i-1].b + Random.Range(-delta, delta), cols[i-1].a + Random.Range(-delta, delta));
			}
		}*/

		CalcNoise();


		//texture.SetPixels32(cols);
		//texture.Apply();

		material.SetTexture ("_MixTex", noiseTex);
	}

	void CalcNoise() {
        float y = 0.0F;
		float ranR = Random.value;
		float ranG = Random.value;
		float ranB = Random.value;
		float ranA = Random.value;
        while (y < noiseTex.height) {
            float x = 0.0F;
            while (x < noiseTex.width) {
                float xCoord = x / noiseTex.width * delta;
                float yCoord = y / noiseTex.height * delta;
                float sampleR = 0.25f * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * x * ranR) + 1) * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * y * ranG) + 1);
				float sampleG = 0.25f * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * x * ranG) + 1) * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * y * ranB) + 1);
				float sampleB = 0.25f * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * x * ranB) + 1) * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * y * ranA) + 1);
				float sampleA = 0.25f * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * x * ranA) + 1) * (Mathf.Sin(Mathf.PerlinNoise(xCoord, yCoord) * y * ranR) + 1);
                pix[(int)(y * noiseTex.width + x)] = new Color(sampleR, sampleG, sampleB, sampleA);

                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
