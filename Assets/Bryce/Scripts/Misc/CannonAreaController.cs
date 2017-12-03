using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAreaController : MonoBehaviour {
	private bool followCamera;

	public static bool canShootCannon;

	public Camera camera;
	
	public GameObject cannon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canShootCannon) {
			if (Input.GetKeyDown("e") && !followCamera) {
				followCamera = true;
			}
		}

		if (followCamera) {
			cannon.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, 0);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == 8 || other.gameObject.layer == 9) {
			canShootCannon = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.layer == 8 || other.gameObject.layer == 9) {
			canShootCannon = false;
			followCamera = false;
		}
	}
}
