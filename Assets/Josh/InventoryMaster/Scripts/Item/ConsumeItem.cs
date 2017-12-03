using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ConsumeItem : MonoBehaviour, IPointerDownHandler
{
    public Item item;
    private static Tooltip tooltip;
    public ItemType[] itemTypeOfSlot;
    public static EquipmentSystem equipmentSystem;
    public GameObject duplication;
    public static GameObject mainInventory;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        item = GetComponent<ItemOnObject>().item;
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
            player.GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();

        if (GameObject.FindGameObjectWithTag("MainInventory") != null)
            mainInventory = GameObject.FindGameObjectWithTag("MainInventory");

        
    }

    public void OnPointerDown(PointerEventData data)
    {
        
        //Get the player's inventory
        Inventory inventory = transform.parent.parent.parent.GetComponent<Inventory>();

        if (equipmentSystem != null)
            itemTypeOfSlot = equipmentSystem.itemTypeOfSlots;

        //Set if the current item should be consumed (or equipped)
        bool consumable = item.itemType == ItemType.Consumable;

        //If we press the right mouse button to consume this item
        if (data.button == PointerEventData.InputButton.Right)
        {
            
            //Check the Equipment System (and equip this item to a slot in it if there is an available slot that fits the type)
            if (!consumable && equipmentSystem != null)
            {
                //Iterate through all the equipment slots
                for (int i = 0; i < equipmentSystem.numberOfSlots; i++)
                {
                    //If the current equipment system slot's type corresponds to the current item's type
                    if (itemTypeOfSlot[i].Equals(item.itemType))
                    {
                        PlayerBehaviorController playerController = player.GetComponent<PlayerBehaviorController>();

                        //Equip the item
                        inventory.EquipItem(item);

                        //If the current equipment slot is not empty
                        if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            GameObject otherItemFromCharacterSystem = equipmentSystem.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                            
                            /* 
                            10-14-17 B.C.
                            I commented out this line as it didn't seem to be doing anything outside of throwing a 
                            Unity warning for not being used. 
                            */
                            //Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;

                            if (item.itemType != ItemType.Backpack)
                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                            if (item.itemType == ItemType.Weapon)
                                playerController.DetachWeapon();
                            
                                otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                if (this.gameObject.transform.parent.parent.parent.GetComponent<Hotbar>() != null)
                                    createDuplication(otherItemFromCharacterSystem);

                                
                        }

                        //For handling weapon object equipping to player's hand
                        if (item.itemType == ItemType.Weapon)
                        {
                            
                            playerController.SetWeaponEquipped(true, item.weaponType);
                            //Now add the weapon to the player's hand
                            playerController.AttachWeapon(item.itemModel);
                        }

                        this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

                        equipmentSystem.gameObject.GetComponent<Inventory>().updateItemList();
                        inventory.updateItemList();

                        if (duplication != null)
                            Destroy(duplication.gameObject);
                        break;
                    }
                }


            }
            //For consuming an item, rather than equipping
            else if (consumable)
            {

                Item itemFromDup = null;
                if (duplication != null)
                    itemFromDup = duplication.GetComponent<ItemOnObject>().item;

                inventory.ConsumeItem(item);

                item.itemValue--;
                if (itemFromDup != null)
                {
                    duplication.GetComponent<ItemOnObject>().item.itemValue--;
                    if (itemFromDup.itemValue <= 0)
                    {
                        if (tooltip != null)
                            tooltip.deactivateTooltip();
                        inventory.deleteItemFromInventory(item);
                        Destroy(duplication.gameObject); 
                    }
                }
                if (item.itemValue <= 0)
                {
                    if (tooltip != null)
                        tooltip.deactivateTooltip();
                    inventory.deleteItemFromInventory(item);
                    Destroy(this.gameObject);                        
                }

            }
                
        }
    }    

    //Consumes an item used in the hotbar
    public void consumeIt()
    {
        Inventory inventory = transform.parent.parent.parent.GetComponent<Inventory>();

        bool gearable = false;

        if (GameObject.FindGameObjectWithTag("EquipmentSystem") != null)
            equipmentSystem = player.GetComponent<PlayerInventory>().characterSystem.GetComponent<EquipmentSystem>();

        if (equipmentSystem != null)
            itemTypeOfSlot = equipmentSystem.itemTypeOfSlots;

        Item itemFromDup = null;
        if (duplication != null)
            itemFromDup = duplication.GetComponent<ItemOnObject>().item;       

        bool stop = false;
        if (equipmentSystem != null)
        {
            
            for (int i = 0; i < equipmentSystem.numberOfSlots; i++)
            {
                if (itemTypeOfSlot[i].Equals(item.itemType))
                {
                    if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount == 0)
                    {
                        stop = true;
                        this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        equipmentSystem.gameObject.GetComponent<Inventory>().updateItemList();
                        inventory.updateItemList();
                        inventory.EquipItem(item); //Possibly for altering player attributes, not really wanted though


                        gearable = true;
                        if (duplication != null)
                            Destroy(duplication.gameObject);
                        break;
                    }
                }
            }

            if (!stop)
            {
                for (int i = 0; i < equipmentSystem.numberOfSlots; i++)
                {
                    if (itemTypeOfSlot[i].Equals(item.itemType))
                    {
                        if (equipmentSystem.transform.GetChild(1).GetChild(i).childCount != 0)
                        {
                            GameObject otherItemFromCharacterSystem = equipmentSystem.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                            Item otherSlotItem = otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item;

                            
                            inventory.EquipItem(item);
                            

                            if (item.itemType != ItemType.Backpack)
                                inventory.UnEquipItem1(otherItemFromCharacterSystem.GetComponent<ItemOnObject>().item);
                            
                            if (this == null)
                            {
                                GameObject dropItem = (GameObject)Instantiate(otherSlotItem.itemModel);
                                dropItem.AddComponent<PickUpItem>();
                                dropItem.GetComponent<PickUpItem>().item = otherSlotItem;
                                dropItem.transform.localPosition = player.transform.localPosition;
                                inventory.OnUpdateItemList();
                            }
                            else
                            {
                                otherItemFromCharacterSystem.transform.SetParent(this.transform.parent);
                                otherItemFromCharacterSystem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                                if (this.gameObject.transform.parent.parent.parent.GetComponent<Hotbar>() != null)
                                    createDuplication(otherItemFromCharacterSystem);

                                this.gameObject.transform.SetParent(equipmentSystem.transform.GetChild(1).GetChild(i));
                                this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            }

                            gearable = true;
                            if (duplication != null)
                                Destroy(duplication.gameObject);
                            equipmentSystem.gameObject.GetComponent<Inventory>().updateItemList();
                            inventory.OnUpdateItemList();
                            break;                           
                        }
                    }
                }
            }


        }
        if (!gearable)
        {

            if (duplication != null)
                itemFromDup = duplication.GetComponent<ItemOnObject>().item;

            inventory.ConsumeItem(item);


            item.itemValue--;
            if (itemFromDup != null)
            {
                duplication.GetComponent<ItemOnObject>().item.itemValue--;
                if (itemFromDup.itemValue <= 0)
                {
                    if (tooltip != null)
                        tooltip.deactivateTooltip();
                    inventory.deleteItemFromInventory(item);
                    Destroy(duplication.gameObject);

                }
            }
            if (item.itemValue <= 0)
            {
                if (tooltip != null)
                    tooltip.deactivateTooltip();
                inventory.deleteItemFromInventory(item);
                Destroy(this.gameObject); 
            }

        }        
    }

    public void createDuplication(GameObject Item)
    {
        Item item = Item.GetComponent<ItemOnObject>().item;
        GameObject dup = mainInventory.GetComponent<Inventory>().addItemToInventory(item.itemID, item.itemValue);
        Item.GetComponent<ConsumeItem>().duplication = dup;
        dup.GetComponent<ConsumeItem>().duplication = Item;
    }
}
