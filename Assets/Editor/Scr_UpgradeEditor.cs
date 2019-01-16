using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scr_UpgradeEditor : EditorWindow
{
    private Scr_UpgradeList inventoryItemList;
    private int viewIndex = 1;
    private int recipeNum = 0;

    [MenuItem("Window/Upgrade Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Scr_UpgradeEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("objectPath"))
        {
            string ObjectPath = "Assets/Resources/Data/Data_UpgradeList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_UpgradeList)) as Scr_UpgradeList;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_UpgradeList asset = ScriptableObject.CreateInstance<Scr_UpgradeList>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/Data_UpgradeList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.UpgradeList = new List<Scr_UpgradeInfo>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("objectPath", relPath);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Upgrade Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (inventoryItemList != null)
        {
            PrintTopMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load upgrade list.");
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
            if (viewIndex < inventoryItemList.UpgradeList.Count)
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

        if (inventoryItemList.UpgradeList.Count > 0)
        {
            UpgradeListMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This Upgrade List is Empty.");
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("+ Add Recipe", GUILayout.ExpandWidth(false)))
        {
            AddRecipe();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Remove Recipe", GUILayout.ExpandWidth(false)))
        {
            RemoveRecipe();
        }

        GUILayout.EndHorizontal();
    }

    void AddUpgrade()
    {
        Scr_UpgradeInfo newUpgrade = new Scr_UpgradeInfo();
        newUpgrade.m_name = "New Upgrade";
        inventoryItemList.UpgradeList.Add(newUpgrade);
        viewIndex = inventoryItemList.UpgradeList.Count;
        inventoryItemList.UpgradeList[viewIndex - 1].resourceNameList.Add("Fuel");
        inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList.Add(0);
        inventoryItemList.UpgradeList[viewIndex - 1].resourceNameList.Add("Iron");
        inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList.Add(0);
    }

    void DeleteUpgrade(int index)
    {
        inventoryItemList.UpgradeList.RemoveAt(index);
    }

    void AddRecipe()
    {
        inventoryItemList.UpgradeList[viewIndex - 1].recipeList.Add(0);
        recipeNum += 1;
    }

    void RemoveRecipe()
    {
        inventoryItemList.UpgradeList[viewIndex - 1].recipeList.RemoveAt(recipeNum - 1);
        recipeNum -= 1;
    }

    void UpgradeListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Upgrade", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.UpgradeList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.UpgradeList.Count.ToString() + " Upgrades", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        string[] _choices = new string[inventoryItemList.UpgradeList.Count];
        for (int i = 0; i < inventoryItemList.UpgradeList.Count; i++)
        {
            _choices[i] = inventoryItemList.UpgradeList[i].m_name;
        }

        int _choicesIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choicesIndex, _choices) + 1;

        GUILayout.Space(10);
        inventoryItemList.UpgradeList[viewIndex - 1].m_name = EditorGUILayout.TextField("Upgrade Name", inventoryItemList.UpgradeList[viewIndex - 1].m_name as string);

        GUILayout.Space(10);
        GUILayout.Label("Description");
        inventoryItemList.UpgradeList[viewIndex - 1].m_info = EditorGUILayout.TextArea(inventoryItemList.UpgradeList[viewIndex - 1].m_info as string);

        GUILayout.Space(10);
        GUILayout.Label("Requirements");
        inventoryItemList.UpgradeList[viewIndex - 1].m_requirements = EditorGUILayout.TextArea(inventoryItemList.UpgradeList[viewIndex - 1].m_requirements as string);

        GUILayout.Space(10);
        GUILayout.Label("Resources Needed");

        inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList[0] = EditorGUILayout.IntField("Fuel", inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList[0]);
        inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList[1] = EditorGUILayout.IntField("Iron", inventoryItemList.UpgradeList[viewIndex - 1].resourceAmountList[1]);

        GUILayout.Space(10);
        GUILayout.Label("Recipes Unlocked");

        for (int i = 0; i < inventoryItemList.UpgradeList[viewIndex - 1].recipeList.Count; i++)
        {
            inventoryItemList.UpgradeList[viewIndex - 1].recipeList[i] = EditorGUILayout.IntField(i.ToString(), inventoryItemList.UpgradeList[viewIndex - 1].recipeList[i]);
        }
    }
}
