using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TreasureCube : MonoBehaviour
{
    [Header("Interaction Properties")]
    [SerializeField] private float rotSpeed;

    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(Vector3.right, rotY, Space.World);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}