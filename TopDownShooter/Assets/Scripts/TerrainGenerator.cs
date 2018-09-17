using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

	public float depth = 20;
	public int width = 256;
	public int height = 256;

	public float scale = 20f;

	public Texture2D[] textures;

	public Vector2 offset;

	public GameObject[] trees;

	private float lastOffset;
	public float treesOffset;

	void Start(){
		Terrain terrain = GetComponent<Terrain>();
		offset = new Vector2(Random.Range(0f, 9999f),Random.Range(0f, 9999f));
		terrain.terrainData = GenerateTerrain(terrain.terrainData);
	}

	TerrainData GenerateTerrain (TerrainData terrainData){
		terrainData.size = new Vector3(width, depth, height);
		terrainData.SetHeights(0,0, GenerateHeights());
		return terrainData;
	}

	float[,] GenerateHeights(){
		float[,] heights = new float[width,height];

		for( int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				heights[x,y] = CalculateHeight(x,y);
			}
		}

		return heights;
	}

	float CalculateHeight (int x, int y){
		float xCoord = (float)x / width * scale + offset.x;
		float yCoord = (float)y / height * scale + offset.y;
		return Mathf.PerlinNoise(xCoord, yCoord);
	}
}
