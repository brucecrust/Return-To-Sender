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
	public GameObject[] terrainList;
	private GameObject[] instatiatedTerrainList;

	public GameObject player;

	// Used to check if user settings are correct.
	private bool loadedAllTiles, correctUserSettings, debugged;

	private void Start() {
		// Pull all prefabs from named folder.
		objectList = Resources.LoadAll(nameOfFolder, typeof(UnityEngine.GameObject));

		// Set length of arrays.
		terrainList = new GameObject[objectList.Length];
		instatiatedTerrainList = new GameObject[terrainList.Length];

		// Loop through the prefabs, cast them to game objects, and add them to an array.
		for (int c = 0; c < objectList.Length; c++) {
			Object o = objectList[c];
			GameObject checkForComponent = (GameObject)o;
			if (checkForComponent.GetComponent<Terrain>()) {
				terrainList[c] = checkForComponent;
			}
		}

		// Loop through and create terrains.
		for (int i = 0; i < terrainList.Length; i++) {
			int z = i % resolution;
			int x = i / resolution;
			CreateTerrain(i, x, z);
		}
	}

	private void Update() {
		if (instatiatedTerrainList.Length == terrainList.Length) {
			loadedAllTiles = true;
		}

		if (loadedAllTiles) {
			if (instatiatedTerrainList.Length != (resolution * resolution)) {
				correctUserSettings = false;
			} else {
				correctUserSettings = true;
			}
		}

		if (correctUserSettings) {
			// Check distance from player to a given terrain. Activate/deactivate based on the resulting distance and user settings.
			foreach (GameObject o in instatiatedTerrainList) {
				if (Vector3.Distance(
					player.transform.position, o.transform.position) > (o.GetComponent<Terrain>().terrainData.size.x * chunksToLoad)
					) {
						if (o.activeSelf) {
							o.SetActive(false);
						}
					} else {
						if (!o.activeSelf) {
							o.SetActive(true);
						}
					}
			}
		} else {
			if (!debugged) {
				print (
					"<color=red> Incorrect map resolution. Set value to: </color>" + "<b>" + Mathf.Sqrt(instatiatedTerrainList.Length) + "</b>"
					);
				debugged = true;
			}
		}
	}

	private void CreateTerrain(int i, int x, int z) {
		// Instantiate game object and set its parent.
		GameObject o = Instantiate(terrainList[i]) as GameObject;
		o.transform.parent = transform;

		// Set local position and scale relative to the parent object.
		o.transform.localPosition = new Vector3(
			x * o.GetComponent<Terrain>().terrainData.size.x, 0, z * o.GetComponent<Terrain>().terrainData.size.z
			);
		o.transform.localScale = Vector3.one * o.GetComponent<Terrain>().terrainData.size.x;

		// Add terrain to a final list to track its real-world positions.
		instatiatedTerrainList[i] = o;
	}
}
