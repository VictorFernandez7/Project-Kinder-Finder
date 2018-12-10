using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Scr_CraftInfo
{
    public string m_name;

    [Header("Requirements")]
    public int m_fuel;
    public int m_iron;
}

public class Scr_CraftData : ScriptableObject
{
    public List<Scr_CraftInfo> craftList;
}

public class Scr_CraftEditor : EditorWindow
{
    private Scr_CraftData inventoryItemList;
    private int viewIndex = 1;

    [MenuItem("Window/Craft Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Scr_CraftEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("objectPath"))
        {
            string ObjectPath = "Assets/Resources/Data/craftList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_CraftData)) as Scr_CraftData;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_CraftData asset = ScriptableObject.CreateInstance<Scr_CraftData>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/craftList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.craftList = new List<Scr_CraftInfo>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("objectPath", relPath);
            }
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Craft Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (inventoryItemList != null)
        {
            PrintTopMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load craft list.");
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItemList);
        }
    }

    void PrintTopMenu()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        if (GUILayout.Button("<- Prev", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex > 1)
                viewIndex -= 1;
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Next ->", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex < inventoryItemList.craftList.Count)
                viewIndex += 1;
        }

        GUILayout.Space(60);

        if (GUILayout.Button("+ Add Craft", GUILayout.ExpandWidth(false)))
        {
            AddUpgrade();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete Craft", GUILayout.ExpandWidth(false)))
        {
            DeleteUpgrade(viewIndex - 1);
        }

        GUILayout.EndHorizontal();

        if (inventoryItemList.craftList.Count > 0)
        {
            UpgradeListMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This Craft List is Empty.");
        }
    }

    void AddUpgrade()
    {
        Scr_CraftInfo newCraft = new Scr_CraftInfo();
        newCraft.m_name = "New Craft";
        inventoryItemList.craftList.Add(newCraft);
        viewIndex = inventoryItemList.craftList.Count;
    }

    void DeleteUpgrade(int index)
    {
        inventoryItemList.craftList.RemoveAt(index);
    }

    void UpgradeListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Craft", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.craftList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.craftList.Count.ToString() + " Craft", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        string[] _choices = new string[inventoryItemList.craftList.Count];
        for (int i = 0; i < inventoryItemList.craftList.Count; i++)
        {
            _choices[i] = inventoryItemList.craftList[i].m_name;
        }

        int _choicesIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choicesIndex, _choices) + 1;

        GUILayout.Space(10);
        inventoryItemList.craftList[viewIndex - 1].m_name = EditorGUILayout.TextField("Name", inventoryItemList.craftList[viewIndex - 1].m_name as string);

        GUILayout.Space(10);
        GUILayout.Label("Requirements");

        inventoryItemList.craftList[viewIndex - 1].m_fuel = EditorGUILayout.IntField("Fuel", inventoryItemList.craftList[viewIndex - 1].m_fuel);
        inventoryItemList.craftList[viewIndex - 1].m_iron = EditorGUILayout.IntField("Iron", inventoryItemList.craftList[viewIndex - 1].m_iron);
    }
}

