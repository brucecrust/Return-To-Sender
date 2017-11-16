<<<<<<< HEAD
﻿using System;
=======
﻿using System;
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
<<<<<<< HEAD
public class PlayerStatistics : MonoBehaviour {

    public float positionX, positionY, positionZ;

=======
public class PlayerStatistics : MonoBehaviour {

    public float positionX, positionY, positionZ;

>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
    public float maxHP;
    public float currentHP;
    public float maxStamina;
    public float currentStamina;

    //The experience the player has gained
    public float XP;
    public int playerLevel;

    //The base stat values governing the player's skills
    public float baseStrength;
    public float baseDefense;
    public float baseAgility;
    public float baseLuck;

    //Total values (current) for each of the above base attributes, after all calculations factored in
    public float curStrength;
    public float curDefense;
    public float curAgility;
    public float curLuck;

    //The weight currently carried | The total weight that can be carried
    public float weight;
    public float maxWeight;
    
}
