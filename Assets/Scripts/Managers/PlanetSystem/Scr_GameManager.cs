using UnityEngine;

public class Scr_GameManager : MonoBehaviour
{
    [Header("Game Start Conditions")]


    [Header("Vortex Spawn")]
    [SerializeField] private float ratio;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;

    [Header("References")]
    [SerializeField] public GameObject initialPlanet;
    [SerializeField] public GameObject vortex;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private GameObject playerShip;

    private float initialRatio;
    private Vector3 vortexPosition;

    private void Awake()
    {
        astronaut.GetComponent<Scr_AstronautMovement>().currentPlanet = initialPlanet;
        astronaut.GetComponent<Scr_AstronautMovement>().planetPosition = initialPlanet.transform.position;
        playerShip.GetComponent<Scr_PlayerShipMovement>().currentPlanet = initialPlanet;

        initialRatio = ratio;
    }

    private void Update()
    {
        VortexSpawn();
    }

    private void VortexSpawn()
    {
        initialRatio -= Time.deltaTime;

        if (initialRatio <= 0)
        {
            vortexPosition = new Vector3(Random.Range(-xMax, xMax), Random.Range(-yMax, yMax), 0);

            Instantiate(vortex, vortexPosition, transform.rotation);

            initialRatio = ratio;
        }
    }
}