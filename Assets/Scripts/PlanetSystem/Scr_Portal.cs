using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Portal : MonoBehaviour
{
    private bool onRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
            onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
            onRange = false;
    }

    private void Update()
    {
        if (onRange && Input.GetKeyDown(KeyCode.E))
            Scr_LevelManager.LoadSystemSelection();
    }
}
