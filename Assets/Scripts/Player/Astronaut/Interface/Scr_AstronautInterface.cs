using UnityEngine;

public class Scr_AstronautInterface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject selectionWheel;
    [SerializeField] private Scr_ToolWheel toolWheel;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private float minDistance;
    private string selectedTool;
    private GameObject wheel;

    private void Update()
    {
        InputProcess();

        minDistance = 100;
    }

    private void InputProcess()
    {
        if (Input.GetMouseButtonDown(1) && wheel == null)
        {
            Vector3 desiredPos = new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0);

            wheel = Instantiate(selectionWheel, desiredPos, Quaternion.identity);
            wheel.transform.SetParent(worldCanvas.transform);
            worldCanvas.transform.SetParent(playerShipMovement.currentPlanet.transform);
            toolWheel = wheel.GetComponent<Scr_ToolWheel>();
        }

        if (Input.GetMouseButton(1))
            MouseMovement();

        if (Input.GetMouseButtonUp(1))
            SelectTool();
    }

    private void MouseMovement()
    {
        wheel.transform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i < toolWheel.tools.Length; i++)
        {
            if (Vector2.Distance(toolWheel.tools[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) < minDistance)
                selectedTool = toolWheel.tools[i].name;
        }

        minDistance = 100;

        print(selectedTool);
    }

    private void SelectTool()
    {
        Destroy(wheel);
    }
}