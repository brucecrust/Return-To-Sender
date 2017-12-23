using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour {
	public static void KillObject(GameObject killTarget) {
		killTarget.GetComponent<NPCController>().rootCOG.constraints = RigidbodyConstraints.None;
		if (killTarget.GetComponent<NPCController>()) {
			killTarget.GetComponent<NPCController>().defaultSpringValue = 0;
			killTarget.GetComponent<NPCController>().jointDrive.positionSpring = killTarget.GetComponent<NPCController>().defaultSpringValue;
			foreach(KeyValuePair<ConfigurableJoint, Transform> boneTransforms in NPCController.armatureDictionary) {
				boneTransforms.Key.slerpDrive = killTarget.GetComponent<NPCController>().jointDrive;
			}
		}
	}
}
