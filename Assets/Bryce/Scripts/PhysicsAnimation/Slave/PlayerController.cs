using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float rotateSpeed;

	// Use this for initialization
	void Update () {
		if (Input.GetKey("a")) {
			transform.Rotate(Vector3.right * rotateSpeed);
		}
		if (Input.GetKey("d")) {
			transform.Rotate(-Vector3.right * rotateSpeed);
		}
	}
}
