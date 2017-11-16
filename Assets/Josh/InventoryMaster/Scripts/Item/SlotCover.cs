<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotCover : MonoBehaviour
{

    Inventory inv;
    RectTransform rT;

    // Use this for initialization
    void Start()
    {
        inv = transform.parent.parent.parent.parent.GetComponent<Inventory>();
        rT = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {

        rT.sizeDelta = new Vector3(inv.slotSize, inv.slotSize, 0);
    }
}
=======
﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotCover : MonoBehaviour
{

    Inventory inv;
    RectTransform rT;

    // Use this for initialization
    void Start()
    {
        inv = transform.parent.parent.parent.parent.GetComponent<Inventory>();
        rT = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {

        rT.sizeDelta = new Vector3(inv.slotSize, inv.slotSize, 0);
    }
}
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
