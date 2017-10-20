using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCube : MonoBehaviour {
	public float cubeForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("j")) {
			GetComponent<Rigidbody>().AddForce(transform.forward * cubeForce, ForceMode.Impulse);
		}
	}
}
