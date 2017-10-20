using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveSpeed, jumpSpeed, sprintSpeed, sprintIncreaseRate;
	public float accelerationForce, accelerationRate, accelerationMaximum;
	private float originalAccelerationForce, originalAccelerationRate, originalAccelerationMaximum;
	public float sprintCounter;
	public float sprintTimer, sprintTimerRate;
	private bool isSprinting, resetSprintTimer, onGround;
	public GameObject cameraPosition;
	public Camera camera;

	private SpringUpOnCollision spring;


	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringUpOnCollision>();

		Cursor.visible = false;

		sprintCounter = sprintTimer;
		originalAccelerationForce = accelerationForce;
		originalAccelerationRate = accelerationRate;
		originalAccelerationMaximum = accelerationMaximum;
	}
	
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

        //transform.Translate(x, 0, 0);
        //	transform.Translate(0, 0, z);

		spring.rootBone.transform.Translate(x, 0, 0);
		spring.rootBone.transform.Translate(0, 0, z);

		spring.rootBone.eulerAngles = new Vector3(spring.rootBone.eulerAngles.x, camera.transform.eulerAngles.y, spring.rootBone.eulerAngles.z);

		if (Input.GetKey("w")) {
			if (onGround) {
				spring.rootBone.GetComponent<Rigidbody>().AddForce(spring.rootBone.transform.forward * accelerationForce, ForceMode.Impulse);
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
			spring.rootBone.transform.Translate(0, 0, -z);
			//spring.rootBone.GetComponent<Rigidbody>().AddForce(spring.rootBone.transform.forward * accelerationForce, ForceMode.Impulse);
		}


		if (Input.GetKeyUp("space")) {
			spring.rootBone.GetComponent<Rigidbody>().AddForce(-spring.rootBone.right * jumpSpeed, ForceMode.Impulse);
			onGround = false;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting) {
			accelerationMaximum += sprintSpeed;
			accelerationRate += sprintIncreaseRate;
			isSprinting = true;
			AnimationController.masterAnimator.speed = 1.5f;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isSprinting = false;
			accelerationMaximum = originalAccelerationMaximum;
			if (accelerationForce > accelerationMaximum) {
				accelerationForce = accelerationMaximum;
			}
			AnimationController.masterAnimator.speed = 1f;
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Terrain") {
			onGround = true;
		}
		if (other.gameObject.tag == "GroundType") {
			onGround = true;
			spring.rootBone.GetComponent<Rigidbody>().velocity = Vector3.zero;
			spring.rootBone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

	void OnCollisionExit(Collision other) {
		spring.rootBone.GetComponent<Rigidbody>().velocity = Vector3.zero;
		spring.rootBone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
}
