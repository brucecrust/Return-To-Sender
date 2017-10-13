using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float playerSpeed;
	public float cameraSpeed;
	private Quaternion targetRotation;
	public Camera camera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		var mouseX = Input.GetAxis("Mouse X");
		var mouseY = Input.GetAxis("Mouse Y");	

		Vector3 movementVector = new Vector3(-mouseY, mouseX, 0);
		Vector3 playerMovement = new Vector3(0, mouseX, 0);

		//camera.transform.eulerAngles += movementVector * cameraSpeed * Time.deltaTime;
		//transform.eulerAngles = new Vector3(0, camera.transform.eulerAngles.y, 0);

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
	}
}
