using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPlayer : MonoBehaviour {

    [Header("Extractor Properties")]
    [SerializeField] private ExtractorType extractorType;
    [SerializeField] private Scr_OreExtractor oreExtractor;
    [SerializeField] private Scr_GasExtractor gasExtractor;

    public enum ExtractorType
    {
        oreExtractor,
        gasExtractor
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astronaut")
        {
            switch (extractorType)
            {
                case ExtractorType.oreExtractor:
                    oreExtractor.playerNear = true;
                    break;

                case ExtractorType.gasExtractor:
                    gasExtractor.playerNear = true;
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
                    oreExtractor.playerNear = false;
                    break;

                case ExtractorType.gasExtractor:
                    gasExtractor.playerNear = false;
                    break;
            }
        }
    }
}