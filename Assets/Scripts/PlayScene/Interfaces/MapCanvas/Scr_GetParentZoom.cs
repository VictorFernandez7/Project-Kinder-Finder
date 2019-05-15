using UnityEngine;

public class Scr_GetParentZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera parentCamera;

    private void Update()
    {
        GetComponent<Camera>().orthographicSize = parentCamera.orthographicSize;
    }
}