﻿using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_DevTools : MonoBehaviour
{
    [Header("Bullet Time")]
    [Range(0, 1)] [SerializeField] private float slowAmount;
    [SerializeField] private KeyCode BT_key;

    [Header("Reload Scene")]
    [SerializeField] private KeyCode RS_key;

    [Header("Reset Technologies")]
    [SerializeField] private KeyCode RT_key;

    [Header("Unlock Liquid Tool")]
    [SerializeField] private KeyCode LT_key;

    private GameObject playerShip;
    private bool devSlow; 

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetKey(BT_key))
            BulletTime(true);

        else
            BulletTime(false);

        if (Input.GetKeyDown(RS_key))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BulletTime(bool active)
    {
        if (active)
        {
            Time.timeScale = slowAmount;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = false;
            devSlow = true;
        }

        else if(devSlow == true)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += 0.25f * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
                playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = true;
                devSlow = false;
            }
        }
    }
}