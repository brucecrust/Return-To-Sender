using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public enum InteractionType
    {
        Pickup,
        Use,
        Converse
    }
    public InteractionType m_interactionType;

    private Animator anim;
    private GameObject player;

    //Use type variables

    /* 
    10-14-17 B.C.
    I commented out this line as it didn't seem to be doing anything outside of throwing a 
    Unity warning for not being used. 
    */
    //private bool stateUpdated = false;
    private bool lastStateOpen = false;
    private float finishUseDistance = 3f;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        //For opening/closing the storage inventory after the Open/Close animation has played (Closes inventory as soon as close anim starts, rather than finish)
        if (m_interactionType == InteractionType.Use)
        {
            UpdateUsableObject();
        }

        
	}

    public void Interact()
    {

        switch (m_interactionType)
        {

            case InteractionType.Converse:
                //GetComponent<Converse>().StartDialogue();
                break;

            case InteractionType.Use:
                Use();
                break;
        }

    }

    private void Use()
    {
        if (tag == "OpenableContainer")
        {
            //Play the open animation
            GetComponent<Animator>().SetTrigger("Open");
        }
    }

    private void FinishUse()
    {
        if (tag == "OpenableContainer")
        {
            //Play the close animation
            GetComponent<Animator>().SetTrigger("Close");

        }
    }

    private void UpdateUsableObject()
    {
        //Just opening the chest for the first time
        if (!GetComponent<StorageInventory>().inventory.activeSelf && anim.GetBool("StaticOpened") && !lastStateOpen)
        {
            GetComponent<StorageInventory>().OpenInventory();
            player.GetComponent<PlayerInventory>().OpenInventory();
            lastStateOpen = true;
        }
        else if (GetComponent<StorageInventory>().inventory.activeSelf && !anim.GetBool("StaticOpened") && lastStateOpen)
        {
            GetComponent<StorageInventory>().CloseInventory();
            player.GetComponent<PlayerInventory>().CloseInventory();
            lastStateOpen = false;
        }

        //Determine if the Container should be closed now
        if (GetComponent<StorageInventory>().inventory.activeSelf && (Vector3.Distance(transform.position, player.transform.position) > finishUseDistance
            || Input.GetKeyDown(KeyCode.Escape)))
            FinishUse();
    }

}
