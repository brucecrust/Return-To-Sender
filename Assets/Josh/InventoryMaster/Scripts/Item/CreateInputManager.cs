<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateInputManager
{

    public static InputManager asset;

#if UNITY_EDITOR
    public static InputManager createInputManager()
    {
        asset = ScriptableObject.CreateInstance<InputManager>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryMaster/Resources/InputManager.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
#endif

}
=======
﻿using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateInputManager
{

    public static InputManager asset;

#if UNITY_EDITOR
    public static InputManager createInputManager()
    {
        asset = ScriptableObject.CreateInstance<InputManager>();

        AssetDatabase.CreateAsset(asset, "Assets/InventoryMaster/Resources/InputManager.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
#endif

}
>>>>>>> 899eeccb8251bbd1771259362b821d44aaac99c7
