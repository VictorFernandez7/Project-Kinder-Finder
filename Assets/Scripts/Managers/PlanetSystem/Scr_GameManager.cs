using UnityEngine;

public class Scr_GameManager : MonoBehaviour
{
    [Header("Experience Values")]
    [SerializeField] public int sightedXP;
    [SerializeField] public int exploredXP;

    [Header("Vortex Spawn")]
    [SerializeField] private float ratio;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;

    [Header("Planet Info")]
    [SerializeField] public GameObject[] planets;

    [Header("Crafting Info")]
    [SerializeField] public bool[] shipCrafts; 
    [SerializeField] public bool[] toolCrafts;
    [SerializeField] public bool[] suitCrafts;

    [Header("References")]
    [SerializeField] public GameObject initialPlanet;
    [SerializeField] public GameObject vortex;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private GameObject playerShip;
    [SerializeField] private Scr_Wheel shipWheel;
    [SerializeField] private Scr_Wheel toolWheel;
    [SerializeField] private Scr_Wheel suitWheel;

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

        shipWheel.unlockedItems = shipCrafts;
        toolWheel.unlockedItems = toolCrafts;
        suitWheel.unlockedItems = suitCrafts;
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

    public void Gravity(bool active)
    {
        if (active)
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_AstroBase>().switchGravity = true;
            }
        }

        else
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].GetComponent<Scr_AstroBase>().switchGravity = false;
            }
        }
    }
}