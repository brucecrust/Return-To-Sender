using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCreatesPrefab : MonoBehaviour {
	public float forceToCreatePrefab;
	public static float requiredForce;
	private bool hasCollided;
	private Component[] childrenList;
	private Renderer renderer;

	// Use this for initialization
	void Start () {
		requiredForce = forceToCreatePrefab;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasCollided) {
			Destroy(GetComponent<MeshRenderer>());
			Destroy(GetComponent<MeshCollider>());
			Destroy(GetComponent<MeshFilter>());

			childrenList = GetComponentsInChildren(typeof(MeshRenderer));
			foreach (MeshRenderer mr in childrenList) {
				if (mr.transform.gameObject.GetComponent<Rigidbody>()) {
					if (mr.transform.gameObject.GetComponent<Renderer>()) {
						renderer = mr.transform.gameObject.GetComponent<Renderer>();
					}
					mr.enabled = true;
					mr.GetComponent<MeshCollider>().enabled = true;
					mr.transform.gameObject.GetComponent<Rigidbody>().mass = (renderer.bounds.size.x + renderer.bounds.size.y) * 15f;
				}
			}
			if (transform.GetComponent<MeshCollider>()) {
				transform.GetComponent<MeshCollider>().enabled = false;
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "DestructibleCollider") {
			if (other.relativeVelocity.magnitude > forceToCreatePrefab) {
				hasCollided = true;
			}
		}
	}
}
