using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scr_DragFuel : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [Header("ItemValues")]
    [SerializeField] private int itemIndex;

    [Header("References")]
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] private Scr_PlayerShipWarehouse playerShipWarehouse;

    private bool dragging;
    private bool onRange;

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        transform.localPosition = Vector3.zero;

        if (onRange && playerShipStats.resourceWarehouse[itemIndex].name == "Fuel")
            Refuel();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
            onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FuelTank"))
            onRange = false;
    }

    private void Update()
    {
        if (dragging)
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private void Refuel()
    {
        playerShipStats.currentFuel += 50;
        playerShipStats.resourceWarehouse[itemIndex] = null;
    }
}
