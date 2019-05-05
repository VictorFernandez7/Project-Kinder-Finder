using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MaterialDetection : MonoBehaviour
{
    [SerializeField] private Scr_LiquidTool liquidTool;
    [SerializeField] private Scr_GasTool gasTool;
    [SerializeField] private DetectionType detectionType;

    public enum DetectionType
    {
        Gas,
        Liquid
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (detectionType)
        {
            case DetectionType.Liquid:
                if (collision.CompareTag("LiquidZone"))
                {
                    liquidTool.zone = collision.gameObject;
                }
                break;

            case DetectionType.Gas:
                if (collision.CompareTag("GasZone"))
                {
                    gasTool.zone = collision.gameObject;
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (detectionType)
        {
            case DetectionType.Liquid:
                if (collision.CompareTag("LiquidZone"))
                {
                    liquidTool.zone = null;
                }
                break;

            case DetectionType.Gas:
                if (collision.CompareTag("GasZone"))
                {
                    gasTool.zone = null;
                }
                break;
        }

    }
}
