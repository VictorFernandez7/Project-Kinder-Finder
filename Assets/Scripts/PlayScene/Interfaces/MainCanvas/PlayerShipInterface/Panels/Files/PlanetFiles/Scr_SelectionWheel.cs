using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Scr_SelectionWheel : MonoBehaviour
{
    [Header("Interaction Properties")]
    [SerializeField] private float rotSpeed;

    private void OnMouseDrag()
    {
        float rotZ = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.forward, -rotZ, Space.World);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}