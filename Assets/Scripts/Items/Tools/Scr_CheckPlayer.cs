using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPlayer : MonoBehaviour {

    [SerializeField] private Scr_GasExtractor scr_gasExtractor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Astronaut")
        {
            scr_gasExtractor.playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
        {
            scr_gasExtractor.playerNear = false;
        }
    }
}
