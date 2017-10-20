using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionController : MonoBehaviour {
	private Rigidbody rb;
	private bool hasCollided;
	public List<GameObject> neighborList;
	RaycastHit hit;
	Ray ray;
	private Dictionary<Transform, Vector3> originalPositionList;

	// Use this for initialization
	void Start () {
		originalPositionList = new Dictionary<Transform, Vector3>();

		neighborList = new List<GameObject>();

		if (GetComponent<Rigidbody>()) {
			rb = GetComponent<Rigidbody>();
		}

		ray.origin = transform.position;

		for (float rayCastModifier = 5f; rayCastModifier < 25f; rayCastModifier += 5f) {
			ray.direction = -Vector3.right;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
				}
			}

			ray.direction = Vector3.right;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
				}
			}

			ray.direction = Vector3.forward;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
				}
			}

			ray.direction = -Vector3.forward;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
				}
			}

			ray.direction = Vector3.up;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
				}
			}
			ray.direction = -Vector3.up;
			if (Physics.Raycast(ray, out hit, rayCastModifier)) {
				if (hit.transform.gameObject.GetComponent<Rigidbody>()) {
					neighborList.Add(hit.transform.gameObject);
					if (!originalPositionList.ContainsKey(hit.transform)) {
						originalPositionList.Add(hit.transform, hit.transform.position);
					}
				}
			}
		}

		if (!originalPositionList.ContainsKey(transform)) {
			originalPositionList.Add(transform, transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*ray.direction = -Vector3.up;
		if (Physics.Raycast(ray, out hit, 1f)) {
			if (!hit.transform.gameObject.GetComponent<Rigidbody>()) {
				rb.isKinematic = false;
				rb.useGravity = true;
			}
		}*/

		foreach(KeyValuePair<Transform, Vector3> t in originalPositionList) {
			if (t.Key.position != t.Value) {
				rb.isKinematic = false;
				rb.useGravity = true;
			}
		}

		if (hasCollided) {
			if (rb.isKinematic) {
				rb.isKinematic = false;
				rb.useGravity = true;
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}
			foreach (GameObject o in neighborList) {
				if (o.GetComponent<Rigidbody>().isKinematic) {
					o.GetComponent<Rigidbody>().isKinematic = false;
					o.GetComponent<Rigidbody>().useGravity = true;
					o.GetComponent<Rigidbody>().velocity = Vector3.zero;
					o.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
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
		//transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
		//transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
}
