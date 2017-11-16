using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : MonoBehaviour {
	// Name of the folder to pull prefabs from. Must be inside of the resources folder.
	public string nameOfFolder;

	// The resolution of the map, e.g. a value of 7 would be squared and result in 49 tiles.
	public int resolution;

	// How many tiles to load near the player.
	public float chunksToLoad;

	// Lists to track and edit terrain prefabs.
	private Object[] objectList;
	private GameObject[] assetTerrainList;
	public List<GameObject> instantiatedTerrainList;

	public GameObject player;

	// Used to check if user settings are correct.
	private bool correctUserSettings, debugged;

	private void Start() {
		// Pull all prefabs from named folder.
		objectList = Resources.LoadAll(nameOfFolder, typeof(UnityEngine.GameObject));

		// Set length of arrays.
		assetTerrainList = new GameObject[objectList.Length];
		instantiatedTerrainList = new List<GameObject>();

		// Loop through the prefabs, cast them to game objects, and add them to an array.
		for (int c = 0; c < objectList.Length; c++) {
			Object o = objectList[c];
			GameObject checkForComponent = (GameObject)o;
			if (checkForComponent.GetComponent<Terrain>()) {
				assetTerrainList[c] = checkForComponent;
			}
		}
	}

	private void Update() {
		foreach(GameObject o in instantiatedTerrainList.ToArray()) {
			if (o == null) {
				instantiatedTerrainList.Remove(o);
			}
		}

		if (assetTerrainList.Length != (resolution * resolution)) {
			correctUserSettings = false;
		} else {
			correctUserSettings = true;
		}

		if (correctUserSettings) {
			// Check distance from player to a given terrain. Activate/deactivate based on the resulting distance and user settings.
			foreach (GameObject o in instantiatedTerrainList.ToArray()) {
				if (Vector3.Distance(o.transform.position, player.transform.position) > o.GetComponent<Terrain>().terrainData.size.x * chunksToLoad) {
					instantiatedTerrainList.Remove(o);
					Destroy(o);
				}
			}

			foreach (GameObject o in assetTerrainList) {
				if (Vector3.Distance(o.transform.position, player.transform.position) < o.GetComponent<Terrain>().terrainData.size.x * chunksToLoad) {
					if (GameObject.Find(o.name + "(Clone)") == null) {
						CreateTerrain(o);
					}
				}
			}

		} else {
			if (!debugged) {
				print (
					"<color=red> Incorrect map resolution. Set value to: </color>" + "<b>" + Mathf.Sqrt(assetTerrainList.Length) + "</b>"
					);
				debugged = true;
			}
		}
	}

	private void CreateTerrain(GameObject newTerrain) {
		GameObject o = Instantiate(newTerrain) as GameObject;
		o.transform.parent = transform;
		o.transform.localScale = Vector3.one * o.GetComponent<Terrain>().terrainData.size.x;
		instantiatedTerrainList.Add(o);
	}
}
