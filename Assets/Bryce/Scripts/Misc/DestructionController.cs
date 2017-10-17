using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour {
	private Rigidbody rb;
	private bool hasCollided;
	public List<GameObject> neighborList;
	RaycastHit hit;
	Ray ray;

	// Use this for initialization
	void Start () {
		neighborList = new List<GameObject>();

		if (GetComponent<Rigidbody>()) {
			rb = GetComponent<Rigidbody>();
		}
		ray.origin = transform.position;

		ray.direction = -Vector3.right;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}

		ray.direction = Vector3.right;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}

		ray.direction = Vector3.forward;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}

		ray.direction = -Vector3.forward;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}

		ray.direction = Vector3.up;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}
		ray.direction = -Vector3.up;
		if (Physics.Raycast(ray, out hit, 25f)) {
			if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
				neighborList.Add(hit.transform.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hasCollided) {
			if (rb.isKinematic) {
				rb.isKinematic = false;
				rb.useGravity = true;
				rb.velocity = Vector3.zero;
			}
			foreach (GameObject o in neighborList) {
				if (o.GetComponent<Rigidbody>().isKinematic) {
					o.GetComponent<Rigidbody>().isKinematic = false;
					o.GetComponent<Rigidbody>().useGravity = true;
					o.GetComponent<Rigidbody>().velocity = Vector3.zero;
				}
			}
			hasCollided = false;
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "DestructibleCollider") {
			if (other.relativeVelocity.magnitude > CollisionCreatesPrefab.requiredForce) {
				hasCollided = true;
			}
		}
	}
}
