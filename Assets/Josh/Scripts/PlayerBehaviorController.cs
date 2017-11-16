using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttachWeapon(GameObject weaponModel)
    {
        Debug.Log("TODO: Attach the weapon that has just been selected in the inventory here");
    }

    public void DetachWeapon()
    {
        Debug.Log("TODO: Detach any weapon held by the player here");

    }

    public void SetWeaponEquipped(bool equipped, WeaponType type = WeaponType.None)
    {
        if (equipped)
        {
            Debug.Log("TODO: handle animation state for holding weapon of type WeaponType");
            //TODO: Set a bool here indicating the player is holding a weapon
            //TODO: set an animation integer on, so that the player changes stance to holding the weapon type
        }
        else
            Debug.Log("TODO: set the player equipped bool back to unarmed");
        
    }
}
