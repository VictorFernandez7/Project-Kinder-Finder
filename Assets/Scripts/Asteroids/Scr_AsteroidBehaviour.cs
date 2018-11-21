using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_AsteroidBehaviour : MonoBehaviour
{
    [Header("Mining Parameters")]
    [SerializeField] float attachingDistance;

    [Header("Control Properties")]
    [SerializeField] private KeyCode miningKey;
    [SerializeField] private string message;
    [SerializeField] private float fontSize;
    [SerializeField] private bool bold;

    private bool attached;
    private GameObject playerShip;
    private TextMeshProUGUI messageText;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        messageText = GetComponentInChildren<TextMeshProUGUI>();

        messageText.text = "";
        messageText.fontSize = fontSize;

        if (bold)
            messageText.fontStyle = FontStyles.Bold;
    }

    private void Update()
    {
        if (attached)
        {
            //Vector3 direction = new Vector3(playerShip.transform.position.x - transform.position.x, playerShip.transform.position.y - transform.position.y, playerShip.transform.position.z - transform.position.z);

            if (Vector3.Distance(transform.position, playerShip.transform.position) > attachingDistance)
                playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, transform.position, Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            messageText.text = message;

            if (Input.GetKeyDown(miningKey))
            {
                attached = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            messageText.text = "";
        }
    }
}