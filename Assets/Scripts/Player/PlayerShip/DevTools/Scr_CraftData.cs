using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scr_CraftData
{
    public string m_name;

    [Header("Requirements")]
    public int m_fuel;
    public int m_iron;
}

public class Scr_CraftList : ScriptableObject
{
    public List<Scr_CraftData> craftList;
}

public class Scr_CraftEditor : EditorWindow
{
    private Scr_CraftList inventoryItemList;
    private int viewIndex = 1;

    [MenuItem("Window/Craft Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Scr_CraftEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string ObjectPath = "Assets/Resources/Data/craftList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_CraftList)) as Scr_CraftList;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_CraftList asset = ScriptableObject.CreateInstance<Scr_CraftList>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/craftList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.craftList = new List<Scr_CraftData>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("ObjectPath", relPath);
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

        if (GUILayout.Button("+ Add Upgrade", GUILayout.ExpandWidth(false)))
        {
            AddUpgrade();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete Upgrade", GUILayout.ExpandWidth(false)))
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
            GUILayout.Label("This Upgrade List is Empty.");
        }
    }

    void AddUpgrade()
    {
        Scr_CraftData newCraft = new Scr_CraftData();
        newCraft.m_name = "New Upgrade";
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
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Upgrade", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.craftList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.craftList.Count.ToString() + " Upgrades", "", GUILayout.ExpandWidth(false));
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
