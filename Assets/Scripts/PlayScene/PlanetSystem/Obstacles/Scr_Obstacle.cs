using UnityEngine;

public class Scr_Obstacle : MonoBehaviour
{
    [Header("Select Obstacle Type")]
    [SerializeField] private ObstacleType obstacleType;

    [Header("Obstacle Parameters")]
    [SerializeField] public float damage;
    [SerializeField] public float impulseForce;

    [Header("References")]
    [SerializeField] private GameObject lavaVisuals;
    [SerializeField] private GameObject iceSpikesVisuals;
    [SerializeField] private GameObject toxicLiquidVisuals;

    private enum ObstacleType
    {
        Lava,
        IceSpikes,
        ToxicLiquid
    }

    void Start()
    {
        ChangeVisuals();
    }

    private void ChangeVisuals()
    {
        switch (obstacleType)
        {
            case ObstacleType.Lava:
                lavaVisuals.SetActive(true);
                iceSpikesVisuals.SetActive(false);
                toxicLiquidVisuals.SetActive(false);
                break;
            case ObstacleType.IceSpikes:
                lavaVisuals.SetActive(false);
                iceSpikesVisuals.SetActive(true);
                toxicLiquidVisuals.SetActive(false);
                break;
            case ObstacleType.ToxicLiquid:
                lavaVisuals.SetActive(false);
                iceSpikesVisuals.SetActive(false);
                toxicLiquidVisuals.SetActive(true);
                break;
        }
    }
}