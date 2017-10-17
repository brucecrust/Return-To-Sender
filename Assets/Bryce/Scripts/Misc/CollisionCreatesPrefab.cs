using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCreatesPrefab : MonoBehaviour {
	public float forceToCreatePrefab;
	public static float requiredForce;
	private bool hasCollided;
	private Component[] childrenList;

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
					mr.enabled = true;
					mr.transform.gameObject.GetComponent<Rigidbody>().mass = Random.Range(200, 300);
				}
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
