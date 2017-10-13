using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour {
	public Transform ragdollTarget;
	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {
		transform.position = ragdollTarget.position;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
