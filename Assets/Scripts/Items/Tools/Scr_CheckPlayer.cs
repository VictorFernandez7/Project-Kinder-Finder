using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPlayer : MonoBehaviour {

    [SerializeField] private ExtractorType extractorType;
    [SerializeField] private Scr_OreExtractor scr_oreExtractor;
    [SerializeField] private Scr_GasExtractor scr_gasExtractor;

    public enum ExtractorType
    {
        oreExtractor,
        gasExtractor
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Astronaut")
        {
            switch (extractorType)
            {
                case ExtractorType.oreExtractor:
                    scr_oreExtractor.playerNear = true;
                    break;

                case ExtractorType.gasExtractor:
                    scr_gasExtractor.playerNear = true;
                    break;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
        {
            switch (extractorType)
            {
                case ExtractorType.oreExtractor:
                    scr_oreExtractor.playerNear = false;
                    break;

                case ExtractorType.gasExtractor:
                    scr_gasExtractor.playerNear = false;
                    break;
            }
        }
    }
}
