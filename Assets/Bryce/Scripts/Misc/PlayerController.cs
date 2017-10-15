using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public float rotateSpeed;
	public float jumpSpeed;

	private Rigidbody playerRb;

	// Use this for initialization
	void Start () {
		playerRb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

		if (Input.GetKey("space")) {
			playerRb.AddForce(Vector3.up * jumpSpeed);
		}
	}
}
