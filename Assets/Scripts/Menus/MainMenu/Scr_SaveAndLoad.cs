using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

public class Scr_SaveAndLoad : MonoBehaviour
{
    [SerializeField] private GameObject astronaut;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_ReferenceManager referenceManager;

    private GameInfo gameInfo;

    public enum SaveSlot
    {
        slot1,
        slot2,
        slot3
    }

    public void SaveXML (SaveSlot saveSlot)
    {
        SaveGame();

        string fileName = "";

        XmlSerializer serializer = new XmlSerializer(typeof(GameInfo));

        switch (saveSlot)
        {
            case SaveSlot.slot1:
                fileName = "/GameData1.xml";
                break;

            case SaveSlot.slot2:
                fileName = "/GameData2.xml";
                break;

            case SaveSlot.slot3:
                fileName = "/GameData3.xml";
                break;
        }

        FileStream file = File.Create(Application.persistentDataPath + fileName);
        serializer.Serialize(file, gameInfo);
        file.Close();
    }

    public void LoadXML (SaveSlot saveSlot)
    {
        string fileName = "";

        XmlSerializer serializer = new XmlSerializer(typeof(GameInfo));

        switch (saveSlot)
        {
            case SaveSlot.slot1:
                fileName = "/GameData1.xml";
                break;

            case SaveSlot.slot2:
                fileName = "/GameData2.xml";
                break;

            case SaveSlot.slot3:
                fileName = "/GameData3.xml";
                break;
        }

        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
        gameInfo = (GameInfo)serializer.Deserialize(file);

        LoadGame();
    }

    private void SaveGame()
    {
        gameInfo.playerInfo.astronautPosition = astronaut.transform.position;
        gameInfo.playerInfo.astronautRotation = astronaut.transform.rotation;

        gameInfo.playerInfo.playershipPosition = playerShip.transform.position;
        gameInfo.playerInfo.playerShipRotation = playerShip.transform.rotation;

        for(int i = 0; i < gameManager.planets.Length; i++)
        {
            gameInfo.planetInfo.system1PlanetPosition.Add(gameManager.planets[i].transform.position);
            gameInfo.planetInfo.system1PlanetRotation.Add(gameManager.planets[i].transform.rotation);
        }

        gameInfo.inventoryInfo.resourceName.Clear();

        for (int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            if (playerShipStats.resourceWarehouse[i])
                gameInfo.inventoryInfo.resourceName.Add(playerShipStats.resourceWarehouse[i].name);

            else
                gameInfo.inventoryInfo.resourceName.Add("null");
        }
    }

    private void LoadGame()
    {
        astronaut.transform.position = gameInfo.playerInfo.astronautPosition;
        astronaut.transform.rotation = gameInfo.playerInfo.astronautRotation;

        playerShip.transform.position = gameInfo.playerInfo.playershipPosition;
        playerShip.transform.rotation = gameInfo.playerInfo.playerShipRotation;

        for (int i = 0; i < gameManager.planets.Length; i++)
        {
            gameManager.planets[i].transform.position = gameInfo.planetInfo.system1PlanetPosition[i];
            gameManager.planets[i].transform.rotation = gameInfo.planetInfo.system1PlanetRotation[i];
        }

        for (int i = 0; i < playerShipStats.resourceWarehouse.Length; i++)
        {
            if (gameInfo.inventoryInfo.resourceName[i] != "null")
            {
                for (int j = 0; j < referenceManager.Resources.Length; i++)
                {
                    if (gameInfo.inventoryInfo.resourceName[i] == referenceManager.Resources[j].name)
                        playerShipStats.resourceWarehouse[i] = referenceManager.Resources[j];
                }
            }
        }
    }
}



[System.Serializable]
public class GameInfo
{
    public PlayerInfo playerInfo;
    public PlanetInfo planetInfo;
    public InventoryInfo inventoryInfo;
}

[System.Serializable]
public class PlanetInfo
{
    public List<Vector3> system1PlanetPosition = new List<Vector3>();
    public List<Quaternion> system1PlanetRotation = new List<Quaternion>();
}

[System.Serializable]
public class PlayerInfo
{
    public Vector3 astronautPosition;
    public Quaternion astronautRotation;

    public Vector3 playershipPosition;
    public Quaternion playerShipRotation;
}

[System.Serializable]
public class InventoryInfo
{
    public List<string> resourceName;
}
