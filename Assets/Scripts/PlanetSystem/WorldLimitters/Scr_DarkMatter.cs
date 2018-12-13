using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DarkMatter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D boxCollider1;
    [SerializeField] private BoxCollider2D boxCollider2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            if (boxCollider1.enabled)
            {
                boxCollider1.enabled = false;
                boxCollider2.enabled = true;
            }

            else
            {

            }
        }
    }
}