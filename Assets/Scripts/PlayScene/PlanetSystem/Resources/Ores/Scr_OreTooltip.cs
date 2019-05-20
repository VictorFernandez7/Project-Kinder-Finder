using UnityEngine;

public class Scr_OreTooltip : MonoBehaviour
{
    [Header("Paramaters")]
    [SerializeField] private float scaleRatio;
    [SerializeField] private float posRatio;

    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        transform.localScale = Vector3.one * mainCamera.orthographicSize * scaleRatio;
        transform.localPosition = new Vector2(0, initialPos.y + (mainCamera.orthographicSize / posRatio));
    }
}