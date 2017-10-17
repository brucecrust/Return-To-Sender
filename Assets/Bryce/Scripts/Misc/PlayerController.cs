using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveSpeed, rotateSpeed, jumpSpeed, sprintSpeed, sprintIncreaseRate;
	public float accelerationForce, accelerationRate, accelerationMaximum;
	private float originalAccelerationForce, originalAccelerationRate, originalAccelerationMaximum;
	public float sprintCounter;
	public float sprintTimer, sprintTimerRate;
	private bool isSprinting, resetSprintTimer, onGround;

	private Rigidbody playerRb;
	public GameObject cameraPosition;
	public Camera camera;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;

		sprintCounter = sprintTimer;
		originalAccelerationForce = accelerationForce;
		originalAccelerationRate = accelerationRate;
		originalAccelerationMaximum = accelerationMaximum;
		playerRb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		camera.transform.position = cameraPosition.transform.position;
		if (isSprinting) {
			sprintCounter -= Time.fixedDeltaTime;
		}

		if (sprintCounter < 0) {
			resetSprintTimer = true;
			isSprinting = false;
			accelerationMaximum = originalAccelerationMaximum;
			if (accelerationForce > accelerationMaximum) {
				accelerationForce = accelerationMaximum;
			}
		}
		
		if (resetSprintTimer) {
			if (sprintCounter < sprintTimer) {
				sprintCounter += sprintTimerRate;
			} else {
				resetSprintTimer = false;
			}
		}

		var x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * moveSpeed;
        var z = Input.GetAxis("Vertical") * Time.fixedDeltaTime * moveSpeed;

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

		transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y, 0);

		if (Input.GetKey("w")) {
			if (onGround) {
				playerRb.AddForce(transform.forward * accelerationForce, ForceMode.Impulse);
			}

			if (accelerationForce < accelerationMaximum) {
				accelerationForce += accelerationRate;
			}
		}

		if (Input.GetKeyUp("w")) {
			accelerationForce = originalAccelerationForce;
			accelerationRate = originalAccelerationRate;
			accelerationMaximum = originalAccelerationMaximum;
		}

		if (Input.GetKey("s")) {
			playerRb.AddForce(-transform.forward * accelerationForce, ForceMode.Impulse);
		}


		if (Input.GetKeyUp("space") && onGround) {
			playerRb.AddForce(Vector3.up * jumpSpeed);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting) {
			accelerationMaximum += sprintSpeed;
			accelerationRate += sprintIncreaseRate;
			isSprinting = true;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isSprinting = false;
			accelerationMaximum = originalAccelerationMaximum;
			if (accelerationForce > accelerationMaximum) {
				accelerationForce = accelerationMaximum;
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponent<Terrain>() || other.gameObject.tag == "GroundType") {
			onGround = true;
		}
	}

	void OnCollisionExit(Collision other) {
		onGround = false;
	}
}
