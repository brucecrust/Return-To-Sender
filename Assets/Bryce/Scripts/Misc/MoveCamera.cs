using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
	//Variables
	public float speed = 15.0F;
	private float originalSpeed;
	public float rotateSpeed = 8.0F; 
	public float gravity = 20.0F;

	void Start() {
		originalSpeed = speed;
	}

    void Update() {
		if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0) {
			transform.GetComponent<Camera>().fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 10f;
			print(Input.GetAxis("Mouse ScrollWheel"));
		}

		//Jumping
		if (Input.GetKey(KeyCode.Space)) {
			transform.Translate(transform.up * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftControl)) {
			transform.Translate(-transform.up * speed * Time.deltaTime);
		}
		
		//Making the character move
		if (Input.GetKey("w")) {
			transform.Translate(transform.forward * speed * Time.deltaTime);
		}

		if (Input.GetKey("s")) {
			transform.Translate(-transform.forward * speed * Time.deltaTime);
		}

		if (Input.GetKey("a")) {
			transform.Rotate(-transform.up * rotateSpeed  * Time.deltaTime);
		}

		if (Input.GetKey("d")) {
			transform.Rotate(transform.up  * rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey("e")) {
			transform.Rotate(-transform.right * rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey("r")) {
			transform.Rotate(transform.right * rotateSpeed  * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			speed *= 3f;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			speed = originalSpeed;
		}
	}
}
