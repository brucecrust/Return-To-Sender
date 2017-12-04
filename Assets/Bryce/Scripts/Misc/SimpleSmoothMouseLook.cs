    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

public class SimpleSmoothMouseLook : MonoBehaviour {

    private float pitch;
    private float yaw;
    private float camRotSpeed = 1.25f;

    public GameObject targetObject;

    public float distanceFromTarget = 2.5f;

    public float rotationSmoothTime = 0.15f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;


    void Start()
    {

    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        transform.eulerAngles = new Vector3(Mathf.Clamp(pitch, -40f, 85f), transform.eulerAngles.y, transform.eulerAngles.z);

        transform.position = targetObject.transform.position - (transform.forward * distanceFromTarget);
    }
}
