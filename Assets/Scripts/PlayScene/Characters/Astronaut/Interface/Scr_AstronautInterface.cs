﻿using UnityEngine;

public class Scr_AstronautInterface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject selectionWheel;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;
    [SerializeField] private Scr_NarrativeManager narrativeManager;

    private int toolIndex;
    private float minDistance;
    private string selectedTool;
    private GameObject wheel;
    private Scr_ToolWheel toolWheel;
    private Scr_AstronautsActions astronautsActions;

    private void Start()
    {
        astronautsActions = GetComponent<Scr_AstronautsActions>();

        minDistance = 100;
    }

    private void Update()
    {
        if(!narrativeManager.onDialogue)
            InputProcess();
    }

    private void InputProcess()
    {
        if (Input.GetMouseButtonDown(1) && wheel == null)
        {
            Vector3 desiredPos = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, -50);

            wheel = Instantiate(selectionWheel, desiredPos, Quaternion.identity);
            wheel.transform.SetParent(worldCanvas.transform);
            worldCanvas.transform.SetParent(playerShipMovement.currentPlanet.transform);
            toolWheel = wheel.GetComponent<Scr_ToolWheel>();
            toolWheel.wheelAnim.SetBool("Show", true);
            minDistance = 100;
        }

        if (Input.GetMouseButton(1))
        {
            wheel.transform.rotation = mainCamera.transform.rotation;

            if (Vector2.Distance(wheel.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) > 0.15f)
                UpdateSelectedTool();

            else
            {
                toolWheel.toolName.text = "";
                toolIndex = 5;

                for (int j = 0; j < toolWheel.selectionSprites.Length; j++)
                {
                    toolWheel.selectionSprites[j].SetActive(false);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
            SelectTool();
    }

    private void UpdateSelectedTool()
    {
        for (int i = 0; i < toolWheel.unlockedTools.Length; i++)
        {
            if (Vector2.Distance(toolWheel.unlockedTools[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) < minDistance)
            {
                minDistance = Vector2.Distance(toolWheel.unlockedTools[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                selectedTool = toolWheel.unlockedTools[i].name;
                toolIndex = i;

                for (int j = 0; j < toolWheel.selectionSprites.Length; j++)
                {
                    if (j == i)
                        toolWheel.selectionSprites[j].SetActive(true);

                    else
                        toolWheel.selectionSprites[j].SetActive(false);
                }
            }
        }

        minDistance = 100;
        //toolWheel.toolsAnim.SetBool(selectedTool, true);
        toolWheel.toolName.text = selectedTool;
    }

    private void SelectTool()
    {
        toolWheel.wheelAnim.SetBool("Show", false);

        if (toolIndex < 5)
            astronautsActions.TakeTool(toolIndex);

        else
            astronautsActions.NoToolsOnHands();

        Destroy(wheel, 0.5f);
    }
}