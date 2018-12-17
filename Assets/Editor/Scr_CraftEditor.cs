using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            string ObjectPath = "Assets/Resources/Data/CraftList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_CraftData)) as Scr_CraftData;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_CraftData asset = ScriptableObject.CreateInstance<Scr_CraftData>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/CraftList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.CraftList = new List<Scr_CraftInfo>();
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
            if (viewIndex < inventoryItemList.CraftList.Count)
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

        if (inventoryItemList.CraftList.Count > 0)
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
        inventoryItemList.CraftList.Add(newCraft);
        viewIndex = inventoryItemList.CraftList.Count;
    }

    void DeleteUpgrade(int index)
    {
        inventoryItemList.CraftList.RemoveAt(index);
    }

    void UpgradeListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Craft", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.CraftList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.CraftList.Count.ToString() + " Craft", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        string[] _choices = new string[inventoryItemList.CraftList.Count];
        for (int i = 0; i < inventoryItemList.CraftList.Count; i++)
        {
            _choices[i] = inventoryItemList.CraftList[i].m_name;
        }

        int _choicesIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choicesIndex, _choices) + 1;

        GUILayout.Space(10);
        inventoryItemList.CraftList[viewIndex - 1].m_name = EditorGUILayout.TextField("Name", inventoryItemList.CraftList[viewIndex - 1].m_name as string);


        GUILayout.Space(10);
        GUILayout.Label("Description");
        inventoryItemList.CraftList[viewIndex - 1].m_info = EditorGUILayout.TextArea(inventoryItemList.CraftList[viewIndex - 1].m_info as string);


        GUILayout.Space(10);
        GUILayout.Label("Requirements");

        inventoryItemList.CraftList[viewIndex - 1].m_fuel = EditorGUILayout.IntField("Fuel", inventoryItemList.CraftList[viewIndex - 1].m_fuel);
        inventoryItemList.CraftList[viewIndex - 1].m_iron = EditorGUILayout.IntField("Iron", inventoryItemList.CraftList[viewIndex - 1].m_iron);
    }
}

