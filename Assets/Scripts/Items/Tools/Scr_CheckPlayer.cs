using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPlayer : MonoBehaviour {

    [SerializeField] private Scr_OreExtractor scr_oreExtractor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Astronaut")
        {
            scr_oreExtractor.playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
        {
            scr_oreExtractor.playerNear = false;
        }
    }
}
