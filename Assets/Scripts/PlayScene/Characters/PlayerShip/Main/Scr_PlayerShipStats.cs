using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Scr_PlayerShipStats : MonoBehaviour
{
    [Header("Respawn Properties")]
    [SerializeField] private float respawnTime;
    [SerializeField] private float fadeTime;

    [Header("Shield Properties")]
    [SerializeField] public float currentShield;
    [SerializeField] public float maxShield;
    [Range(0, 100)] [SerializeField] private float shieldAlertPercentage;
    [SerializeField] private Color shieldColor0;
    [SerializeField] private Color shieldColor25;
    [SerializeField] private Color shieldColor50;
    [SerializeField] private Color shieldColor75;

    [Header("Fuel Properties")]
    [SerializeField] public float currentFuel;
    [SerializeField] public float maxFuel;
    [Range(0, 100)] [SerializeField] private float fuelAlertPercentage;
    [SerializeField] private float normalConsume;
    [SerializeField] private float boostConsume;
    [SerializeField] private Color fuelColor0;
    [SerializeField] private Color fuelColor25;
    [SerializeField] private Color fuelColor50;
    [SerializeField] private Color fuelColor75;

    [Header("References")]
    [SerializeField] private GameObject shipVisuals;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Animator fadeImage;
    [SerializeField] private Image fuelSliderFill;
    [SerializeField] private Image fuelTankSliderFill;
    [SerializeField] private Image shieldSliderFill;
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Slider fuelTankSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private Scr_GameManager gameManager;
    [SerializeField] public Scr_ReferenceManager referenceManager;
    [SerializeField] public Animator anim_FuelPanel;
    [SerializeField] public Animator anim_ShieldPanel;
    [SerializeField] public Scr_LevelData levelData;
    [SerializeField] public Scr_CraftData craftData;
    [SerializeField] public Scr_LevelUpCanvas levelUpCanvas;
    [SerializeField] public Scr_NarrativeManager narrativeManager;
    [SerializeField] public Scr_AstronautMovement astronautMovement;
    [SerializeField] public Animator astronautAnim;
    [SerializeField] public Scr_AstronautEffects astronautEffects;
    [SerializeField] public Scr_AstronautStats astronautStats;
    [SerializeField] public Scr_MainCamera mainCamera;
    [SerializeField] public Scr_PlayerShipProxCheck playerShipProxCheck;
    [SerializeField] public Scr_PlayerShipHalo playerShipHalo;

    [Header("Sounds")]
    [SerializeField] private SoundDefinition explosion;

    [Header("Inventory")]
    [SerializeField] public GameObject[] resourceWarehouse;

    [HideInInspector] public int lastWarehouseEmpty;
    [HideInInspector] public int experience;
    [HideInInspector] public int level;
    [HideInInspector] public bool gasExtractor;
    [HideInInspector] public bool repairingTool;
    [HideInInspector] public bool jetpack;
    [HideInInspector] public bool inDanger;
  
    private Rigidbody2D rb;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipEffects playerShipEffects;
    private Scr_PlayerShipPrediction playerShipPrediction;
    private bool isRefueled;

    private void Start()
    {
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipEffects = GetComponent<Scr_PlayerShipEffects>();
        playerShipPrediction = GetComponent<Scr_PlayerShipPrediction>();
        rb = GetComponent<Rigidbody2D>();

        fuelSlider.maxValue = maxFuel;
        fuelTankSlider.maxValue = maxFuel;
        shieldSlider.maxValue = maxShield;

        level = 1;
    }

    private void Update()
    {
        Fuel();
        Shield();

        if (Input.GetKeyDown(KeyCode.L))
            GetExperience(35);

        if(currentFuel == maxFuel && !isRefueled)
        {
            narrativeManager.StartDialogue(9);
            isRefueled = true;
        }

        if (currentShield <= 0 && !Scr_PlayerData.dead)
            Death();

        if (currentFuel <= 0 && !playerShipMovement.onGround)
            Death();
    }

    float sliderValue;

    private void Fuel()
    {
        FuelSliderColor();

        sliderValue = Mathf.Lerp(sliderValue, currentFuel, Time.deltaTime);

        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        fuelSlider.value = currentFuel;
        fuelTankSlider.value = sliderValue;

        if (currentFuel <= ((fuelAlertPercentage / 100) * maxFuel))
            anim_FuelPanel.SetBool("Alert", true);

        else
            anim_FuelPanel.SetBool("Alert", false);
    }

    private void Shield()
    {
        ShieldSliderColor();

        currentShield = Mathf.Clamp(currentShield, 0f, maxShield);
        shieldSlider.value = currentShield;

        if (currentShield <= (0.1f * maxShield))
            playerShipMovement.damaged = true;

        else
            playerShipMovement.damaged = false;


        if (currentShield <= ((shieldAlertPercentage / 100) * maxShield))
            anim_ShieldPanel.SetBool("Alert", true);

        else
            anim_ShieldPanel.SetBool("Alert", false);

        if (currentShield <= (0.25 * maxShield))
            playerShipEffects.damaged = true;

        else
            playerShipEffects.damaged = false;
    }

    public void FuelConsumption(bool boost)
    {
        if (boost)
            currentFuel -= boostConsume;

        else
            currentFuel -= normalConsume;
    }

    public void ReFuel(float amount)
    {
        if (currentFuel < maxFuel)
            currentFuel += amount;
    }

    public void RepairShield(float amount)
    {
        if (currentShield < maxShield)
            currentShield += amount;
    }

    private void FuelSliderColor()
    {
        float colorChangeSpeed = 2;

        if (fuelSlider.value >= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor75, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.75f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor50, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.50f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor25, Time.deltaTime * colorChangeSpeed);
        if (fuelSlider.value <= (0.25f * fuelSlider.maxValue))
            fuelSliderFill.color = Color.Lerp(fuelSliderFill.color, fuelColor0, Time.deltaTime * colorChangeSpeed);

        if (fuelTankSlider.value >= (0.75f * fuelTankSlider.maxValue))
            fuelTankSliderFill.color = Color.Lerp(fuelTankSliderFill.color, fuelColor75, Time.deltaTime * colorChangeSpeed);
        if (fuelTankSlider.value <= (0.75f * fuelTankSlider.maxValue))
            fuelTankSliderFill.color = Color.Lerp(fuelTankSliderFill.color, fuelColor50, Time.deltaTime * colorChangeSpeed);
        if (fuelTankSlider.value <= (0.50f * fuelTankSlider.maxValue))
            fuelTankSliderFill.color = Color.Lerp(fuelTankSliderFill.color, fuelColor25, Time.deltaTime * colorChangeSpeed);
        if (fuelTankSlider.value <= (0.25f * fuelTankSlider.maxValue))
            fuelTankSliderFill.color = Color.Lerp(fuelTankSliderFill.color, fuelColor0, Time.deltaTime * colorChangeSpeed);
    }

    private void ShieldSliderColor()
    {
        float colorChangeSpeed = 2;

        if (shieldSlider.value >= (0.75f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor75, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.75f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor50, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.50f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor25, Time.deltaTime * colorChangeSpeed);
        if (shieldSlider.value <= (0.25f * shieldSlider.maxValue))
            shieldSliderFill.color = Color.Lerp(shieldSliderFill.color, shieldColor0, Time.deltaTime * colorChangeSpeed);
    }

    private int savedPrediction;
    private bool diedOnce;

    public void Death()
    {
        if (!diedOnce)
        {
            diedOnce = true;

            if (playerShipMovement.astronautOnBoard)
            {
                playerShipMovement.canControlShip = false;
                playerShipMovement.canRotateShip = false;
                playerShipMovement.landedOnce = false;
                Scr_PlayerData.dead = true;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                shipVisuals.gameObject.SetActive(false);
                playerShipEffects.PlayParticleSystem(playerShipEffects.explosion);
                Scr_MusicManager.Instance.PlaySound(explosion.Sound, 0);
                playerShipEffects.StopAllThrusters();
                collider.enabled = false;
                playerShipProxCheck.ClearInterface(false);
                playerShipHalo.disableHalo = true;
                savedPrediction = playerShipPrediction.predictionTime;
                playerShipPrediction.predictionTime = 0;

                GetComponentInChildren<Scr_PlayerShipDeathCheck>().enabled = false;

                Invoke("Fade", fadeTime);
            }

            else
            {
                astronautStats.visuals.transform.position = new Vector3(astronautStats.visuals.transform.position.x, astronautStats.visuals.transform.position.y, astronautStats.initialVisualPos.z + 50);
                astronautMovement.canMove = false;
                astronautAnim.SetTrigger("Death");

                switch (playerShipMovement.currentPlanet.GetComponent<Scr_Planet>().planetType)
                {
                    case Scr_Planet.PlanetType.EarthLike:
                        astronautEffects.DeathParticles(Scr_AstronautEffects.DeathType.Normal);
                        break;
                    case Scr_Planet.PlanetType.Frozen:
                        astronautEffects.DeathParticles(Scr_AstronautEffects.DeathType.Ice);
                        break;
                    case Scr_Planet.PlanetType.Volcanic:
                        astronautEffects.DeathParticles(Scr_AstronautEffects.DeathType.Fire);
                        break;
                    case Scr_Planet.PlanetType.Arid:
                        astronautEffects.DeathParticles(Scr_AstronautEffects.DeathType.Normal);
                        break;
                    case Scr_Planet.PlanetType.Toxic:
                        astronautEffects.DeathParticles(Scr_AstronautEffects.DeathType.Posion);
                        break;
                }

                Invoke("Fade", fadeTime);
            }

            Invoke("Respawn", respawnTime);
        }

    }

    private float savedRotationSpeed;

    private void Fade()
    {
        fadeImage.SetBool("Fade", false);
        savedRotationSpeed = mainCamera.shipRotationSpeed;
        mainCamera.shipRotationSpeed = 20;

        Invoke("ResoreRotationSpeed", 4.5f);
    }

    private void ResoreRotationSpeed()
    {
        mainCamera.shipRotationSpeed = savedRotationSpeed;
        fadeImage.SetBool("Fade", true);
    }

    private void Respawn()
    {
        if (!playerShipMovement.astronautOnBoard)
            GetComponent<Scr_PlayerShipActions>().astronaut.GetComponent<Scr_AstronautsActions>().EnterShipFromPlanet();

        astronautStats.visuals.transform.position = new Vector3(astronautStats.visuals.transform.position.x, astronautStats.visuals.transform.position.y, astronautStats.initialVisualPos.z);
        currentShield = Scr_PlayerData.checkpointShield;
        playerShipMovement.currentPlanet = Scr_PlayerData.checkpointPlanet.gameObject;
        playerShipMovement.playerShipState = Scr_PlayerShipMovement.PlayerShipState.landed;
        playerShipMovement.snapped = false;
        playerShipMovement.onGround = true;
        transform.SetParent(Scr_PlayerData.checkpointPlanet);
        transform.localRotation = Scr_PlayerData.checkpointPlayershipRotation;
        transform.localPosition = Scr_PlayerData.checkpointPlayershipPosition;
        currentFuel = Scr_PlayerData.checkpointFuel;
        rb.isKinematic = false;
        GetComponent<Scr_PlayerShipActions>().startExitDelay = false;
        GetComponent<Scr_PlayerShipActions>().canExitShip = true;
        GetComponent<Scr_PlayerShipActions>().unlockInteract = true;
        playerShipMovement.canControlShip = true;
        shipVisuals.gameObject.SetActive(true);
        playerShipPrediction.predictionTime = savedPrediction;
        playerShipMovement.snapped = false;

        astronautMovement.canMove = true;

        diedOnce = false;
        Scr_PlayerData.dead = false;
    }

    public void GetExperience (int amount)
    {
        experience += amount;

        if (levelData.LevelList[level].experienceNeeded <= experience)
        {
            experience -= levelData.LevelList[level].experienceNeeded;
            astronautMovement.gameObject.GetComponent<Scr_AstronautEffects>().ConfettiParticles();

            levelUpCanvas.gameObject.SetActive(true);

            if (levelData.LevelList[level].levelRewards.Count == 2)
            {
                for (int i = 0; i < levelData.LevelList[level].levelRewards.Count; i++)
                {
                    craftData.CraftList[levelData.LevelList[level].levelRewards[i]].crafteable = true;
                }

                levelUpCanvas.UpdatePanelInfo(experience, levelData.LevelList[level + 1].experienceNeeded, (level + 1).ToString(), levelData.LevelList[level].levelTitle, false, craftData.CraftList[levelData.LevelList[level].levelRewards[0]].m_name, craftData.CraftList[levelData.LevelList[level].levelRewards[0]].m_icon, craftData.CraftList[levelData.LevelList[level].levelRewards[1]].m_name, craftData.CraftList[levelData.LevelList[level].levelRewards[1]].m_icon);
            }

            else
            {
                craftData.CraftList[levelData.LevelList[level].levelRewards[0]].crafteable = true;
                levelUpCanvas.UpdatePanelInfo(experience, levelData.LevelList[level + 1].experienceNeeded, (level + 1).ToString(), levelData.LevelList[level].levelTitle, true, craftData.CraftList[levelData.LevelList[level].levelRewards[0]].m_name, craftData.CraftList[levelData.LevelList[level].levelRewards[0]].m_icon, null, null);
            }

            UpdateCrafts();
            level += 1;
        }

        experienceSlider.value = (float)experience / (float)levelData.LevelList[level].experienceNeeded;
    }

    public void UpdateCrafts()
    {
        if (craftData.CraftList[1].crafteable)
            gameManager.toolCrafts[1] = true;
        else
            gameManager.toolCrafts[1] = false;

        if (craftData.CraftList[2].crafteable)
            gameManager.toolCrafts[0] = true;
        else
            gameManager.toolCrafts[0] = false;

        if (craftData.CraftList[3].crafteable)
            gameManager.toolCrafts[2] = true;
        else
            gameManager.toolCrafts[2] = false;

        if (craftData.CraftList[4].crafteable)
            gameManager.toolCrafts[3] = true;
        else
            gameManager.toolCrafts[3] = false;

        if (craftData.CraftList[8].crafteable)
            gameManager.toolCrafts[4] = true;
        else
            gameManager.toolCrafts[4] = false;

        if (craftData.CraftList[5].crafteable)
            gameManager.suitCrafts[2] = true;
        else
            gameManager.suitCrafts[2] = false;

        if (craftData.CraftList[6].crafteable)
            gameManager.suitCrafts[1] = true;
        else
            gameManager.suitCrafts[1] = false;

        if (craftData.CraftList[7].crafteable)
            gameManager.suitCrafts[0] = true;
        else
            gameManager.suitCrafts[0] = false;

        if (craftData.CraftList[9].crafteable)
            gameManager.shipCrafts[0] = true;
        else
            gameManager.shipCrafts[0] = false;

        if (craftData.CraftList[10].crafteable)
            gameManager.shipCrafts[3] = true;
        else
            gameManager.shipCrafts[3] = false;

        if (craftData.CraftList[11].crafteable)
            gameManager.shipCrafts[2] = true;
        else
            gameManager.shipCrafts[2] = false;

        if (craftData.CraftList[12].crafteable)
            gameManager.shipCrafts[1] = true;
        else
            gameManager.shipCrafts[1] = false;
    }
}