using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class Scr_Sun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PostProcessVolume postProcessVolume;

    private bool danger;
    private Scr_PlayerShipStats playerShipStats;

    Vignette vignetteLayer;

    Color redColor = Color.red;
    float intensity = 0.8f;

    private void Start()
    {
        playerShipStats = GameObject.Find("PlayerShip").GetComponent<Scr_PlayerShipStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip") && !danger)
        {
            danger = true;
            playerShipStats.inDanger = true;

            postProcessVolume.profile.TryGetSettings(out vignetteLayer);
            vignetteLayer.intensity.value = 0.8f;
            vignetteLayer.color.value = Color.red;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
        {
            playerShipStats.inDanger = false;
            danger = false;

            postProcessVolume.profile.TryGetSettings(out vignetteLayer);
            vignetteLayer.intensity.value = 0.25f;
            vignetteLayer.color.value = Color.black;
        }
    }
}