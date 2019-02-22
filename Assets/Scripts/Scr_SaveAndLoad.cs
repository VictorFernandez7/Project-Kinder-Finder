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
        gameInfo.playerInfo.AstronautPosition = astronaut.transform.position;
        gameInfo.playerInfo.AstronautRotation = astronaut.transform.rotation;

        gameInfo.playerInfo.PlayershipPosition = playerShip.transform.position;
        gameInfo.playerInfo.PlayerShipRotation = playerShip.transform.rotation;

        for(int i = 0; i < gameManager.planets.Length; i++)
        {
            gameInfo.planetInfo.System1PlanetPosition.Add(gameManager.planets[i].transform.position);
            gameInfo.planetInfo.System1PlanetRotation.Add(gameManager.planets[i].transform.rotation);
        }
    }

    private void LoadGame()
    {
        astronaut.transform.position = gameInfo.playerInfo.AstronautPosition;
        astronaut.transform.rotation = gameInfo.playerInfo.AstronautRotation;

        playerShip.transform.position = gameInfo.playerInfo.PlayershipPosition;
        playerShip.transform.rotation = gameInfo.playerInfo.PlayerShipRotation;

        for (int i = 0; i < gameManager.planets.Length; i++)
        {
            gameManager.planets[i].transform.position = gameInfo.planetInfo.System1PlanetPosition[i];
            gameManager.planets[i].transform.rotation = gameInfo.planetInfo.System1PlanetRotation[i];
        }
    }
}



[System.Serializable]
public class GameInfo
{
    public PlayerInfo playerInfo;
    public PlanetInfo planetInfo;
}

[System.Serializable]
public class PlanetInfo
{
    public List<Vector3> System1PlanetPosition = new List<Vector3>();
    public List<Quaternion> System1PlanetRotation = new List<Quaternion>();
}

[System.Serializable]
public class PlayerInfo
{
    public Vector3 AstronautPosition;
    public Quaternion AstronautRotation;

    public Vector3 PlayershipPosition;
    public Quaternion PlayerShipRotation;
}
