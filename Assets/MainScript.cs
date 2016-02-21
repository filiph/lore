#pragma strict

using System.IO;

using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {
	
	public Transform buildingBlock;
	public Transform terrainCube;
	public static int mapWidth = 50;
	public static int mapHeight = 50;
	public static int tileSize = 1;
	
	private int[,] heights = new int[mapWidth,mapHeight];
	
	private void loadHeights() {
		StringReader reader = null; 
 
		TextAsset terdata = (TextAsset)Resources.Load("100sample", typeof(TextAsset));
		
		if (terdata == null) {
			throw new FileNotFoundException("Couldn't find 50sample for some reason!");
		}
		
		// puzdata.text is a string containing the whole file. To read it line-by-line:
		reader = new StringReader(terdata.text);
		if ( reader == null )
		{
		   Debug.Log("terdata not found or not readable");
		}
		else
		{
		   // Read each line from the file
			string line = null;
			int i = 0;
		   	while ( (line = reader.ReadLine()) != null && i < mapHeight ) {
				//Debug.Log("-->" + line);
				string[] heightStrings = line.Split(' ');
				for (var j = 0; j < mapWidth; j++) {
					//Debug.Log(heightStrings[j]);
					int hg = System.Convert.ToInt32(heightStrings[j]);
					heights[i,j] = System.Convert.ToInt32((hg - 20) / 40f );
				}
				i++;
			}
		}
	}
	
	private Vector3 getXzFromIj(int i, int j) {
		// todo: compute only once
		int offsetX = mapWidth * tileSize / 2;
		int offsetZ = mapHeight * tileSize / 2;
		return new Vector3(i * tileSize - offsetX, 0, j * tileSize - offsetZ);
	}
	
	private float getYFromIj(int i, int j) {
		return heights[i,j] / 2f;
	}
	
	
	IEnumerator buildTerrain() {
		for (int i = 0; i < mapWidth; i++) {
			for (int j = 0; j < mapHeight; j++) {
				Transform newTerrainCube = Instantiate(
				            terrainCube, 
				            getXzFromIj(i, j) + Vector3.up * getYFromIj(i, j), 
				            Quaternion.identity
				            ) as Transform;
				newTerrainCube.tag = "Terrain";
				TerrainCubeScript otherScript = newTerrainCube.GetComponent<TerrainCubeScript>();
				otherScript.x = i;
				otherScript.y = j;
			}
		}
		yield return null;
	}
	
	public void placeBlock(RaycastHit hit) {
		//hit.transform.Translate(Vector3.up / 3f);
		TerrainCubeScript otherScript = hit.transform.GetComponent<TerrainCubeScript>();
		int i = otherScript.x;
		int j = otherScript.y;
		
		Vector3 pos = getXzFromIj(i, j);
		float hg = getYFromIj(i, j);
		
		Transform newBlock = Instantiate(
		              buildingBlock, 
		              pos + Vector3.up * hg,
		              Quaternion.identity
		              ) as Transform;
		newBlock.tag = "Block";
	}
	
	void Awake() {
		loadHeights();
		StartCoroutine(buildTerrain());
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
