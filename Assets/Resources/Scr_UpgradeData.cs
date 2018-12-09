using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Scr_UpgradeData 
{
    public string m_name;

    [Header("Requirements")]
    public int m_fuel;
    public int m_iron;
}

public class Scr_UpgradeList : ScriptableObject
{
    public List<Scr_UpgradeData> UpdateList;
}

public class Scr_CreateUpgradeList
{
    [MenuItem("Assets/Create/Upgrade List")]
    public static Scr_UpgradeList Create()
    {
        Scr_UpgradeList asset = ScriptableObject.CreateInstance<Scr_UpgradeList>();
        AssetDatabase.CreateAsset(asset, "Assets/UpgradeList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
