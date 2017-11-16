<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PropObject : MonoBehaviour {

    private bool objBroken = false;
    public GameObject brokenObjVersion;
    private float unitXLength;
    private float unitZLength;

	// Use this for initialization
	void Start () {
        unitXLength = 1;
        unitZLength = 1;
    }
	
	public void SetObjectSize(float xLength, float zLength)
    {
        unitXLength = xLength;
        unitZLength = zLength;
    }

    public Vector2 GetObjectSize()
    {
        return new Vector2(unitXLength, unitZLength);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!objBroken && other.collider.name == "Player")
        {
            Rigidbody otherRB = other.collider.GetComponent<Rigidbody>();
            //If the other object is moving fast enough, break the current object
            if (brokenObjVersion && otherRB && otherRB.velocity.magnitude > 0.3f)
            {
                /* 
                10-14-17 B.C.
                I commented out this line as it didn't seem to be doing anything outside of throwing a 
                Unity warning for not being used. 
                */
                //GameObject newCrate = Instantiate(brokenObjVersion, transform.position, transform.rotation, transform.parent);
                //newCrate.transform.localScale = transform.localScale;
                Destroy(gameObject);

                objBroken = true;
            }
        }
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PropObject : MonoBehaviour {

    private bool objBroken = false;
    public GameObject brokenObjVersion;
    private float unitXLength;
    private float unitZLength;

	// Use this for initialization
	void Start () {
        unitXLength = 1;
        unitZLength = 1;
    }
	
	public void SetObjectSize(float xLength, float zLength)
    {
        unitXLength = xLength;
        unitZLength = zLength;
    }

    public Vector2 GetObjectSize()
    {
        return new Vector2(unitXLength, unitZLength);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!objBroken && other.collider.name == "Player")
        {
            Rigidbody otherRB = other.collider.GetComponent<Rigidbody>();
            //If the other object is moving fast enough, break the current object
            if (brokenObjVersion && otherRB && otherRB.velocity.magnitude > 0.3f)
            {
                /* 
                10-14-17 B.C.
                I commented out this line as it didn't seem to be doing anything outside of throwing a 
                Unity warning for not being used. 
                */
                //GameObject newCrate = Instantiate(brokenObjVersion, transform.position, transform.rotation, transform.parent);
                //newCrate.transform.localScale = transform.localScale;
                Destroy(gameObject);

                objBroken = true;
            }
        }
    }
}
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
