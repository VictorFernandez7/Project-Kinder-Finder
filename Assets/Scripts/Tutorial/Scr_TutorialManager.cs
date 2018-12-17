using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TutorialManager : MonoBehaviour {

    [SerializeField] private GameObject astronaut;
    [SerializeField] private GameObject playerShip;

    [SerializeField] private GameObject warehouseText;
    [SerializeField] private GameObject multitoolText;
    [SerializeField] private GameObject repairedText;

    [SerializeField] private GameObject icon1;
    [SerializeField] private GameObject icon2;
    [SerializeField] private GameObject icon3;

    [SerializeField] private GameObject damagedParticles;
    [SerializeField] private GameObject checkLaser;

    private bool multitool;
    private bool notDamaged;

    private void Update()
    {
        if (!multitool)
            MultitoolCheck();

        if (!notDamaged)
            DamagedCheck();

        if (playerShip.GetComponent<Scr_PlayerShipMovement>().astronautOnBoard)
        {
            checkLaser.SetActive(false);
            checkLaser.GetComponent<Scr_OnMouseCheck>().panel.SetActive(false);
        }

        else
            checkLaser.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerShip.GetComponent<Scr_PlayerShipStats>().currentShield = 100;
        }
    }

    private void MultitoolCheck()
    {
        int icon = 0;

        if (Input.GetButtonDown("Interact"))
        {
            for (int i = 0; i < astronaut.GetComponent<Scr_AstronautStats>().toolSlots.Length; i++)
            {
                if (astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i])
                {
                    if (astronaut.GetComponent<Scr_AstronautStats>().toolSlots[i].name == "Multitool")
                    {
                        multitool = true;
                        icon = i + 1;
                        break;
                    }
                }
            }
        }


        if (multitool)
        {
            switch (icon)
            {
                case 1:
                    icon1.SetActive(true);
                    break;

                case 2:
                    icon2.SetActive(true);
                    break;

                case 3:
                    icon3.SetActive(true);
                    break;
            }

            warehouseText.SetActive(false);
            multitoolText.SetActive(true);
        }
    }

    private void DamagedCheck()
    {
        if(playerShip.GetComponent<Scr_PlayerShipStats>().currentShield >= 98)
        {
            damagedParticles.GetComponent<ParticleSystem>().Stop();
            playerShip.GetComponent<Scr_PlayerShipMovement>().damaged = false;
            multitoolText.SetActive(false);
            repairedText.SetActive(true);
        }
    }
}
