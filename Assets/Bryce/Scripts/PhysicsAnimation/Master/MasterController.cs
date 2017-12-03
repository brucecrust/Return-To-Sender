using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour {
	public Transform ragdollTarget;
	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		transform.position = new Vector3(ragdollTarget.position.x, -50f, ragdollTarget.position.z);
		transform.rotation = ragdollTarget.rotation;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
