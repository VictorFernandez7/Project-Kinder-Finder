using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_MapButton : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Scr_MapManager mapManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        mapManager.OpenMap();
    }
}