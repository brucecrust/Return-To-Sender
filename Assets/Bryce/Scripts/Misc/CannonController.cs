using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
	public Transform cannonBarrelPosition;
	public GameObject cannonBall;
	public float cannonForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && CannonAreaController.canShootCannon) {
			GameObject o = Instantiate(cannonBall) as GameObject;
			o.transform.position = cannonBarrelPosition.position;
			o.GetComponent<Rigidbody>().AddForce(transform.forward * cannonForce, ForceMode.Impulse);
			Destroy(o, 20f);
		}
	}
}
