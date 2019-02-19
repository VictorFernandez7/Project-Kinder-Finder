using UnityEngine;

public class Scr_AstronautInterface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject selectionWheel;
    [SerializeField] private Scr_ToolWheel toolWheel;
    [SerializeField] private Scr_PlayerShipMovement playerShipMovement;

    private float minDistance = 100;
    private string selectedTool;
    private GameObject wheel;

    private void Update()
    {
        InputProcess();
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
            toolWheel.wheelAnim.SetBool("Show", true);
            minDistance = 100;
        }

        if (Input.GetMouseButton(1))
            UpdateSelectedTool();

        if (Input.GetMouseButtonUp(1))
            SelectTool();
    }

    private void UpdateSelectedTool()
    {
        wheel.transform.rotation = mainCamera.transform.rotation;

        for (int i = 0; i < toolWheel.tools.Length; i++)
        {
            if (Vector2.Distance(toolWheel.tools[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition)) < minDistance)
            {
                minDistance = Vector2.Distance(toolWheel.tools[i].transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition));
                selectedTool = toolWheel.tools[i].name;
            }
        }

        minDistance = 100;
        //toolWheel.toolsAnim.SetBool(selectedTool, true);
        toolWheel.toolName.text = selectedTool;
    }

    private void SelectTool()
    {
        toolWheel.wheelAnim.SetBool("Show", false);
        Destroy(wheel, 0.5f);
    }
}