using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Travel : MonoBehaviour
{
    [HideInInspector] public static bool unlockedMultiJump;

    private static Scr_PlayerShipWarehouse playerShipWarehouse;

    public static void Finder()
    {
        playerShipWarehouse = GameObject.Find("RoomManagement").GetComponent<Scr_PlayerShipWarehouse>();
    }

    public static void JumpTravel(bool multiJump, Scr_Levels.Galaxies targetGalaxy, Scr_Levels.LevelToLoad targetSystem)
    {
        if (multiJump && unlockedMultiJump)
        {
            switch (targetGalaxy)
            {
                case Scr_Levels.Galaxies.Galaxy1:
                    if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                    {
                        if ((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to1) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to1;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to2;


                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem1:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem2:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem2);
                                    break;
                            }
                        }
                    }
                    break;

                case Scr_Levels.Galaxies.Galaxy2:
                    if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2)
                    {
                        if ((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to1) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost1to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to1;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost1to2;

                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem3:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem3);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem4:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem4);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem5:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem5);
                                    break;
                            }
                        }
                    }
                    break;

                case Scr_Levels.Galaxies.Galaxy3:
                    if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy3)
                    {
                        if ((Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost0to2) || (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy2 && playerShipWarehouse.jumpCellAmount >= Scr_LevelManager.travelCost1to2))
                        {
                            if (Scr_Levels.currentGalaxy == Scr_Levels.Galaxies.Galaxy1)
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost0to2;

                            else
                                playerShipWarehouse.jumpCellAmount -= Scr_LevelManager.travelCost1to2;

                            switch (targetSystem)
                            {
                                case Scr_Levels.LevelToLoad.PlanetSystem6:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem6);
                                    break;

                                case Scr_Levels.LevelToLoad.PlanetSystem7:
                                    Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem7);
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        else
        {
            switch (targetSystem)
            {
                case Scr_Levels.LevelToLoad.PlanetSystem1:
                    if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem2 && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem1);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem2:
                    if (Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem1 && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem2);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem3:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem4 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem5) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem3);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem4:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem3 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem5) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem4);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem5:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem3 || Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem4) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem5);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem6:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem7) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem6);
                    }
                    break;

                case Scr_Levels.LevelToLoad.PlanetSystem7:
                    if ((Scr_Levels.currentLevel == Scr_Levels.LevelToLoad.PlanetSystem6) && playerShipWarehouse.jumpCellAmount >= 1)
                    {
                        playerShipWarehouse.jumpCellAmount -= 1;
                        Scr_LevelManager.LoadPlanetSystem(Scr_Levels.LevelToLoad.PlanetSystem7);
                    }
                    break;
            }
        }
    }
}
