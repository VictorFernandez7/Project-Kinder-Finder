using UnityEngine;

public class Scr_Ore : MonoBehaviour
{
    [Header("Block Type")]
    [SerializeField] private BlockType blockType;

    [Header("If Ore")]
    [SerializeField] public OreResourceType oreResourceType;

    [Header("If Crystal")]
    [SerializeField] private CrystalResourceType crystalResourceType;

    [Header("Resource Properties")]
    [SerializeField] public float amount;
    [SerializeField] private int experience;

    [Header("Shake Properties")]

    [Header("References")]
    [SerializeField] private GameObject visuals;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Scr_MainCamera mainCamera;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private Scr_ReferenceManager referenceManager;
    [SerializeField] private Scr_PlayerShipStats playerShipStats;
    [SerializeField] public Scr_AstronautsActions astronautsActions;

    [HideInInspector] public GameObject currentResource;

    public float initalAmount;
    public float rest = 0;
    private bool playedOnce;
    private GameObject[] oreVisuals;

    private enum BlockType
    {
        Ore,
        Crystal
    }

    public enum OreResourceType
    {
        Oxygen,
        Fuel,
        Carbon,
        Silicon,
        Iron,
        Aluminum,
        Magnetite,
        Ceramic,
        Termatite
    }

    private enum CrystalResourceType
    {
        Crystal
    }

    private void Start()
    {
        initalAmount = amount;
        oreVisuals = new GameObject[visuals.transform.childCount];

        GetVisualsChildren();

        switch (blockType)
        {
            case BlockType.Ore:
                switch (oreResourceType)
                {
                    case OreResourceType.Oxygen:
                        currentResource = referenceManager.Resources[0];
                        ActivateTargetVisuals(0);
                        break;

                    case OreResourceType.Fuel:
                        currentResource = referenceManager.Resources[1];
                        ActivateTargetVisuals(1);
                        break;

                    case OreResourceType.Carbon:
                        currentResource = referenceManager.Resources[2];
                        ActivateTargetVisuals(2);
                        break;

                    case OreResourceType.Silicon:
                        currentResource = referenceManager.Resources[3];
                        ActivateTargetVisuals(3);
                        break;

                    case OreResourceType.Iron:
                        currentResource = referenceManager.Resources[6];
                        ActivateTargetVisuals(4);
                        break;

                    case OreResourceType.Aluminum:
                        currentResource = referenceManager.Resources[7];
                        ActivateTargetVisuals(5);
                        break;

                    case OreResourceType.Magnetite:
                        currentResource = referenceManager.Resources[8];
                        ActivateTargetVisuals(6);
                        break;

                    case OreResourceType.Ceramic:
                        currentResource = referenceManager.Resources[10];
                        ActivateTargetVisuals(7);
                        break;

                    case OreResourceType.Termatite:
                        currentResource = referenceManager.Resources[11];
                        ActivateTargetVisuals(8);
                        break;
                }

                break;

            case BlockType.Crystal:
                switch (crystalResourceType)
                {
                    case CrystalResourceType.Crystal:

                        break;
                }

                break;
        }
    }

    private void Update()
    {
        if (amount <= 0)
        {
            if (!explosionParticles.isPlaying && !playedOnce)
            {
                for (int i = 0; i < initalAmount; i++) { 
                    GameObject resource = Instantiate(currentResource, transform.position + Vector3.back, transform.rotation);
                    resource.transform.SetParent(transform.parent);
                    playerShipStats.GetExperience(experience);
                }

                astronautsActions.miningSpot = null;

                explosionParticles.Play();
                mainCamera.CameraShake(0.25f, 5, 2);
                visuals.SetActive(false);
                canvas.SetActive(false);
                GetComponent<EdgeCollider2D>().enabled = false;

                playedOnce = true;

                if (oreResourceType == OreResourceType.Fuel)
                {
                    GameObject newOre = Instantiate(referenceManager.Zones[0], transform.position, transform.rotation, transform.parent);
                    newOre.SetActive(false);
                    newOre.GetComponent<Scr_Ore>().mainCamera = mainCamera;
                    newOre.GetComponent<Scr_Ore>().referenceManager = referenceManager;
                    newOre.GetComponent<Scr_Ore>().playerShipStats = playerShipStats;
                    newOre.GetComponent<Scr_Ore>().astronautsActions = astronautsActions;
                    newOre.GetComponent<Scr_Ore>().oreResourceType = OreResourceType.Fuel;
                    newOre.GetComponent<Scr_Ore>().amount = initalAmount;
                    referenceManager.respawnFuelResources.Add(newOre);
                }
                
                Destroy(gameObject, 2.5f);
            }
        }  
        
        if (amount < (initalAmount - rest))
        {
            rest += 1;
            rest = Mathf.Clamp(rest, 0, initalAmount);
        }
    }

    private void GetVisualsChildren()
    {
        for (int i = 0; i < visuals.transform.childCount; i++)
        {
            oreVisuals[i] = visuals.transform.GetChild(i).gameObject;
        }
    }

    private void ActivateTargetVisuals(int index)
    {
        for (int i = 0; i < oreVisuals.Length; i++)
        {
            if (i == index)
                oreVisuals[i].SetActive(true);

            else
                oreVisuals[i].SetActive(false);
        }
    }
}