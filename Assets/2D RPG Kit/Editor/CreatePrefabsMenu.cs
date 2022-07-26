using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePrefabsMenu : Editor
{
    [MenuItem("GameObject / 2D RPG Kit Objects / Player Start", false, 1)]
    private static void CreatePlayerStart()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Player Start.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Battle Area", false, 1)]
    private static void CreateBattleArea()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Battle Area.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Chest", false, 1)]
    private static void CreateChest()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Chest.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Common Events", false, 1)]
    private static void CreateCommonEvents()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Common Events.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Complete Quest", false, 1)]
    private static void CreateCompleteQuest()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Complete Quest.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Quest Object Activator", false, 1)]
    private static void CreateQuestObjectActivator()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Quest Object Activator.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Event Object Activator", false, 1)]
    private static void CreateEventObjectActivator()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Event Object Activator.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Inn Keeper", false, 1)]
    private static void CreateInnKeeper()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Inn Keeper.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Shop Keeper", false, 1)]
    private static void CreateShopKeeper()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Shop Keeper.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / NPC", false, 1)]
    private static void CreateNPC()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/NPC.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Pushable Block", false, 1)]
    private static void CreatePushableBlock()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Pushable Block.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Save Point", false, 1)]
    private static void CreateSavePoint()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Save Point.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Auto Save", false, 1)]
    private static void CreateAutoSave()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Auto Save.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Teleport", false, 1)]
    private static void CreateTeleport()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Objects/Teleport To.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("2D RPG Kit / Game Manager")]
    private static void OpenGameManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Game Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Main/Game Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Audio Manager")]
    private static void OpenAudioManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Audio Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Main/Audio Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Battle Manager")]
    private static void OpenBattleManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Battle Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Main/Battle Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Control Manager")]
    private static void OpenControlManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Control Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/2D RPG Kit/Prefabs/Main/Control Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }
}
