using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scr_LevelEditor : EditorWindow
{
    private Scr_LevelData inventoryItemList;
    private int viewIndex = 1;

    [MenuItem("Window/Level Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Scr_LevelEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("objectPath"))
        {
            string ObjectPath = "Assets/Resources/Data/Data_LevelData.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_LevelData)) as Scr_LevelData;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_LevelData asset = ScriptableObject.CreateInstance<Scr_LevelData>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/Data_LevelData.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.LevelList = new List<Scr_LevelInfo>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("objectPath", relPath);
            }
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (inventoryItemList != null)
        {
            PrintTopMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load level list.");
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
            if (viewIndex < inventoryItemList.LevelList.Count)
                viewIndex += 1;
        }

        GUILayout.Space(60);

        if (GUILayout.Button("+ Add Level", GUILayout.ExpandWidth(false)))
        {
            AddLevel();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete Level", GUILayout.ExpandWidth(false)))
        {
            DeleteLevel(viewIndex - 1);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(195);

        if (GUILayout.Button("+ Add Reward", GUILayout.ExpandWidth(false)))
        {
            AddReward();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete Reward", GUILayout.ExpandWidth(false)))
        {
            RemoveReward();
        }

        GUILayout.EndHorizontal();

        if (inventoryItemList.LevelList.Count > 0)
        {
            LevelListMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This Level List is Empty.");
        }
    }

    void AddLevel()
    {
        Scr_LevelInfo newLevel = new Scr_LevelInfo();
        inventoryItemList.LevelList.Add(newLevel);
        viewIndex = inventoryItemList.LevelList.Count;
        newLevel.m_name = viewIndex.ToString();
    }

    void DeleteLevel(int index)
    {
        inventoryItemList.LevelList.RemoveAt(index);
    }

    void AddReward()
    {
        inventoryItemList.LevelList[viewIndex - 1].levelRewards.Add(000);
    }

    void RemoveReward()
    {
        inventoryItemList.LevelList[viewIndex - 1].levelRewards.RemoveAt(inventoryItemList.LevelList[viewIndex - 1].levelRewards.Count - 1);
    }

    void LevelListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Level", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.LevelList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.LevelList.Count.ToString() + " Level", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        string[] _choices = new string[inventoryItemList.LevelList.Count];
        for (int i = 0; i < inventoryItemList.LevelList.Count; i++)
        {
            _choices[i] = inventoryItemList.LevelList[i].m_name;
        }

        int _choicesIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choicesIndex, _choices) + 1;

        GUILayout.Space(10);
        inventoryItemList.LevelList[viewIndex - 1].levelTitle = EditorGUILayout.TextField("Title", inventoryItemList.LevelList[viewIndex - 1].levelTitle as string);
        inventoryItemList.LevelList[viewIndex - 1].experienceNeeded = EditorGUILayout.IntField("ExperienceNeeded", inventoryItemList.LevelList[viewIndex - 1].experienceNeeded);

        GUILayout.Space(10);
        for(int i = 0; i < inventoryItemList.LevelList[viewIndex - 1].levelRewards.Count; i++)
            inventoryItemList.LevelList[viewIndex - 1].levelRewards[i] = EditorGUILayout.IntField(i.ToString(), inventoryItemList.LevelList[viewIndex - 1].experienceNeeded);
    }
}