using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MaterialGenerator : MonoBehaviour {

	public Material material; 

	public float delta = 1f;
	public float R = 1f;

	private int width=1024;
	private int widthMesh=256;
	private Color[] pix;
	private Texture2D noiseTex;

	public NavMeshSurface nms;
	
	void Start () {

		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.mesh = mesh;

		MeshRenderer mr = GetComponent<MeshRenderer>();
		MeshCollider col = GetComponent<MeshCollider>();

		Vector3[] vertices = new Vector3[widthMesh * widthMesh];
		int[] tri = new int[(widthMesh - 1)*(widthMesh - 1) * 6];
		int j = 0;
		Vector3[] normals = new Vector3[widthMesh * widthMesh];
		Vector2[] uv = new Vector2[widthMesh * widthMesh];

		noiseTex = new Texture2D(width, width);
        pix = new Color[noiseTex.width * noiseTex.height];

		CalcNoise();

		material.SetTexture ("_MixTex", noiseTex);

		for (int i=0; i<widthMesh; i++) {
			for (int k=0;k<widthMesh; k++){
				vertices[i * widthMesh + k] = new Vector3(k, noiseTex.GetPixel(k * (width - 1) / (widthMesh - 1), i * (width - 1) / (widthMesh - 1)).grayscale*R, i);
				normals[i * widthMesh + k] = Vector3.up;
				if (k < widthMesh - 1 && i < widthMesh - 1){
					tri[j] = i * widthMesh + k;
					tri[j+1] = (i + 1) * widthMesh + k;
					tri[j+2] = (i) * widthMesh + k + 1;

					tri[j+3] = (i + 1) * widthMesh + k;
					tri[j+4] = (i + 1) * widthMesh + k + 1;
					tri[j+5] = (i) * widthMesh + k + 1;
					
					j += 6;
				}
				uv[i * widthMesh + k] = new Vector2(vertices[i * widthMesh + k].x / widthMesh, vertices[i * widthMesh + k].z / widthMesh);
			}
		}

		Debug.Log(j);

		mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.uv = uv;

		mr.material = material;

		col.sharedMesh = mesh;

		nms.BuildNavMesh();
	}

	void CalcNoise() {
        float y = 0.0F;
		float ranR = Random.Range(1f, 2f);
		float ranG = Random.Range(1f, 2f);
		float ranB = Random.Range(1f, 2f);
		float ranA = Random.Range(1f, 2f);
		Debug.Log(ranR + " " + ranG + " " + ranB + " " + ranA);
        while (y < noiseTex.height) {
            float x = 0.0F;
            while (x < noiseTex.width) {
                //float xCoord = x / noiseTex.width * delta;
                //float yCoord = y / noiseTex.height * delta;
                float sampleR = Func((x - noiseTex.width / 2) / delta, (y - noiseTex.height / 2) / delta, ranR, ranG);
				float sampleG = Func((x - noiseTex.width / 2) / delta, (y - noiseTex.height / 2) / delta, ranG, ranB);
				float sampleB = Func((x - noiseTex.width / 2) / delta, (y - noiseTex.height / 2) / delta, ranB, ranA);
				float sampleA = Func((x - noiseTex.width / 2) / delta, (y - noiseTex.height / 2) / delta, ranA, ranR);
                pix[(int)(y * noiseTex.width + x)] = new Color(sampleR, sampleG, sampleB, sampleA);

                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

	float Func(float a, float b, float c, float d){
		return Mathf.Pow(Mathf.Sin((Mathf.Sin(b + d) + 2) * a * c / 10 - (Mathf.Cos(b + c) + 2) * d * Mathf.PerlinNoise(a, b)), 2) * (1 - 0.5f * Mathf.PerlinNoise(a * c / 12f, b * d / 12f));
	}
}
