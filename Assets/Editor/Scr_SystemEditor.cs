using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Scr_SystemEditor : EditorWindow
{
    private Scr_SystemData inventoryItemList;
    private int viewIndex = 1;
    private int viewSystem = 1;

    [MenuItem("Window/Planet Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Scr_SystemEditor));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("objectPath"))
        {
            string ObjectPath = "Assets/Resources/Data/Data_SystemList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(ObjectPath, typeof(Scr_SystemData)) as Scr_SystemData;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            Scr_SystemData asset = ScriptableObject.CreateInstance<Scr_SystemData>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Data/Data_SystemList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.SystemList = new List<Scr_PlanetData>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("objectPath", relPath);
            }
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Planet Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (inventoryItemList != null)
        {
            PrintTopMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load planet list.");
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

        if (GUILayout.Button("<- Prev System", GUILayout.ExpandWidth(false)))
        {
            if (viewSystem > 1)
                viewSystem -= 1;
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Next System ->", GUILayout.ExpandWidth(false)))
        {
            if (viewSystem < inventoryItemList.SystemList.Count)
                viewSystem += 1;
        }

        GUILayout.Space(60);

        if (GUILayout.Button("+ Add System", GUILayout.ExpandWidth(false)))
        {
            AddSystem();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete System", GUILayout.ExpandWidth(false)))
        {
            DeleteSystem(viewSystem - 1);
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(17);

        if (GUILayout.Button("<- Prev Planet", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex > 1)
                viewIndex -= 1;
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Next Planet ->", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex < inventoryItemList.SystemList[viewSystem - 1].PlanetList.Count)
                viewIndex += 1;
        }

        GUILayout.Space(76);

        if (GUILayout.Button("+ Add Planet", GUILayout.ExpandWidth(false)))
        {
            AddPlanet(viewSystem - 1);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("- Delete Planet", GUILayout.ExpandWidth(false)))
        {
            DeletePlanet(viewSystem - 1, viewIndex - 1);
        }

        GUILayout.EndHorizontal();

        if (inventoryItemList.SystemList.Count > 0)
        {
            PlanetListMenu();
        }

        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This Planet List is Empty.");
        }
    }

    void AddPlanet(int system)
    {
        Scr_PlanetInfo newPlanet = new Scr_PlanetInfo();
        newPlanet.m_name = "New Planet";
        inventoryItemList.SystemList[system].PlanetList.Add(newPlanet);
        viewIndex = inventoryItemList.SystemList[system].PlanetList.Count;
    }

    void DeletePlanet(int system, int index)
    {
        inventoryItemList.SystemList[system].PlanetList.RemoveAt(index);
    }

    void AddSystem()
    {
        Scr_PlanetData newSystem = new Scr_PlanetData();
        newSystem.m_name = "New System";
        newSystem.PlanetList = new List<Scr_PlanetInfo>();
        inventoryItemList.SystemList.Add(newSystem);
        viewSystem = inventoryItemList.SystemList.Count;
        AddPlanet(viewSystem - 1);
    }

    void DeleteSystem(int system)
    {
        inventoryItemList.SystemList.RemoveAt(system);
    }

    void PlanetListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewSystem = Mathf.Clamp(EditorGUILayout.IntField("Current System", viewSystem, GUILayout.ExpandWidth(false)), 1, inventoryItemList.SystemList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.SystemList.Count.ToString() + " Systems", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Planet", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.SystemList[viewSystem - 1].PlanetList.Count);
        EditorGUILayout.LabelField("of " + inventoryItemList.SystemList[viewSystem - 1].PlanetList.Count.ToString() + " Planets", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        string[] _choices = new string[inventoryItemList.SystemList.Count];
        for (int i = 0; i < inventoryItemList.SystemList.Count; i++)
        {
            _choices[i] = inventoryItemList.SystemList[i].m_name;
        }

        int _choicesIndex = viewSystem - 1;
        viewSystem = EditorGUILayout.Popup(_choicesIndex, _choices) + 1;

        string[] _choices2 = new string[inventoryItemList.SystemList[viewSystem - 1].PlanetList.Count];
        for (int i = 0; i < inventoryItemList.SystemList[viewSystem - 1].PlanetList.Count; i++)
        {
            _choices2[i] = inventoryItemList.SystemList[viewSystem - 1].PlanetList[i].m_name;
        }

        int _choicesIndex2 = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choicesIndex2, _choices2) + 1;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("System", EditorStyles.boldLabel);

        GUILayout.Space(10);
        inventoryItemList.SystemList[viewSystem - 1].m_name = EditorGUILayout.TextField("Name", inventoryItemList.SystemList[viewSystem - 1].m_name as string);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Planet", EditorStyles.boldLabel);

        GUILayout.Space(10);
        inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_name = EditorGUILayout.TextField("Name", inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_name as string);

        GUILayout.Space(10);
        inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_temperature = EditorGUILayout.FloatField("Temperature", inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_temperature);
        inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_oxygen = EditorGUILayout.Toggle("Oxygen", inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_oxygen);
        inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_gravity = EditorGUILayout.FloatField("Oxygen", inventoryItemList.SystemList[viewSystem - 1].PlanetList[viewIndex - 1].m_gravity);
    }
}
