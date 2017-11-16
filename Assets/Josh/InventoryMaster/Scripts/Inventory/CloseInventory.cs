<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseInventory : MonoBehaviour, IPointerDownHandler
{

    Inventory inv;
    void Start()
    {
        inv = transform.parent.GetComponent<Inventory>();

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inv.closeInventory();
        }
    }
}
=======
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseInventory : MonoBehaviour, IPointerDownHandler
{

    Inventory inv;
    void Start()
    {
        inv = transform.parent.GetComponent<Inventory>();

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inv.closeInventory();
        }
    }
}
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
