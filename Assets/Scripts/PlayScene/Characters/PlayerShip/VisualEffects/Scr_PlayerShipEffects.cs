using UnityEngine;

public class Scr_PlayerShipEffects : MonoBehaviour
{
    [Header("Warming System")]
    [SerializeField] private float warmingPower;

    [Header("Landing / Taking Off Parameters")]
    [SerializeField] private float landingPower;
    [SerializeField] private float takingOffPower;

    [Header("In Space Parameters")]
    [SerializeField] private float thrusterPower;
    [SerializeField] private float turboPower;
    [SerializeField] private float auxiliarThrusterPower;

    [Header("Star Parameters")]
    [SerializeField] private float inSpaceEmission;
    [SerializeField] private float inPlanetEmission;
    [SerializeField] private Vector3 inSpaceSize;
    [SerializeField] private Vector3 inPlanetSize;

    [Header("Particle References")]
    [SerializeField] private ParticleSystem mainThruster;
    [SerializeField] private ParticleSystem leftThruster;
    [SerializeField] private ParticleSystem rightThruster;
    [SerializeField] private ParticleSystem leftPropulsor;
    [SerializeField] private ParticleSystem rightPropulsor;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private ParticleSystem takingOffSlam;
    [SerializeField] private ParticleSystem takingOffSmoke;
    [SerializeField] private ParticleSystem damagedSmoke;
    [SerializeField] private ParticleSystem landingSlam;
    [SerializeField] private ParticleSystem stars;

    [HideInInspector] public bool warming;
    [HideInInspector] public bool turbo;
    [HideInInspector] public bool damaged;

    private float desiredEmission;
    private Scr_PlayerShipMovement playerShipMovement;

    private void Start()
    {
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
    }

    private void Update()
    {
        StarControl();
        DamageControl();
        ThrusterPlayControl();
        ThrustersEmissionControl();
    }

    private void StarControl()
    {
        var emission = stars.emission;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            emission.rateOverTime = inPlanetEmission;
            stars.transform.localScale = Vector3.Lerp(stars.transform.localScale, inPlanetSize, Time.deltaTime);
        }

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            emission.rateOverTime = inSpaceEmission;
            stars.transform.localScale = Vector3.Lerp(stars.transform.localScale, inSpaceSize, Time.deltaTime);
        }
    }

    private void ThrustersEmissionControl()
    {
        var mainEmission = mainThruster.emission;
        var leftEmission = leftThruster.emission;
        var rightEmission = rightThruster.emission;

        if (warming)
            desiredEmission = warmingPower;

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
            desiredEmission = takingOffPower;

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            desiredEmission = landingPower;

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            if (turbo)
                desiredEmission = turboPower;

            else
                desiredEmission = thrusterPower;
        }

        mainEmission.rateOverTime = desiredEmission;
        leftEmission.rateOverTime = auxiliarThrusterPower;
        rightEmission.rateOverTime = auxiliarThrusterPower;
    }

    private void ThrusterPlayControl()
    {
        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
        {
            PlayParticleSystem(mainThruster);
            PlayParticleSystem(takingOffSmoke);
        }

        else if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
            PlayParticleSystem(mainThruster);

        // Parar partículas en el evento de salir de la atmósfera
    }

    private void DamageControl()
    {
        if (damaged)
            PlayParticleSystem(damagedSmoke);

        else
            StopParticleSystem(damagedSmoke);
    }

    public void WarmingEffects(bool play) // Cuando se deje de llamar hay que hacer el booleano warming false.
    {
        warming = true;

        if (play)
            PlayParticleSystem(mainThruster);

        else
            StopParticleSystem(mainThruster);
    }

    public void ThrusterEffects(bool play)
    {
        if (play)
            PlayParticleSystem(mainThruster);

        else
            StopParticleSystem(mainThruster);
    }

    public void TurboEffects(bool play) // Cuando se deje de llamar hay que hacer el booleano warming false.
    {
        turbo = true;

        if (play)
        {
            PlayParticleSystem(mainThruster);
            PlayParticleSystem(leftThruster);
            PlayParticleSystem(rightThruster);
        }

        else
        {
            StopParticleSystem(mainThruster);
            StopParticleSystem(leftThruster);
            StopParticleSystem(rightThruster);
        }
    }

    public void TakingOffSlamEffect()
    {
        PlayParticleSystem(takingOffSlam);
    }

    public void LandingSlamEffect()
    {
        PlayParticleSystem(landingSlam);
    }

    public void ExplosionEffect()
    {
        PlayParticleSystem(explosion);
    }

    public void PropulsorEffects(bool left)
    {
        if (left)
            PlayParticleSystem(leftPropulsor);

        else
            PlayParticleSystem(rightPropulsor);
    }

    private void PlayParticleSystem(ParticleSystem desiredParticles)
    {
        if (!desiredParticles.isPlaying)
            desiredParticles.Play();
    }

    private void StopParticleSystem(ParticleSystem desiredParticles)
    {
        if (desiredParticles.isPlaying)
            desiredParticles.Stop();
    }

    /*[Header("Taking Off Effects")]
    [Range(50, 500)] [SerializeField] private float dustMultiplier;
    [Range(350, 4000)] [SerializeField] private float takingOffThrusterPower;

    [Header("In Space Effects")]
    [Range(350, 2000)] [SerializeField] private float inSpaceThrusterPower;

    [Header("Landing Effects")]
    [Range(350, 4000)] [SerializeField] private float landingThrusterPower;

    [Header("Mining Effects")]
    [SerializeField] private float attachedThrusterPower;


    [Header("Warming System")]
    [SerializeField] private float landedThrusterMult;
    [SerializeField] private float landedDustMult;
    [SerializeField] private Color color0;
    [SerializeField] private Color color25;
    [SerializeField] private Color color50;
    [SerializeField] private Color color75;

    [Header("Dynamic Materials")]
    [SerializeField] private Material windowOn;
    [SerializeField] private Material windowOff;

    [Header("References")]
    [SerializeField] private Transform endOfShip;
    [SerializeField] public ParticleSystem thrusterParticles;
    [SerializeField] public ParticleSystem thrusterParticles2;
    [SerializeField] public ParticleSystem thrusterParticles3;
    [SerializeField] public ParticleSystem takingOffDustParticles;
    [SerializeField] public ParticleSystem damageParticles;
    [SerializeField] private GameObject dustParticles;
    [SerializeField] private ParticleSystem atmosphereParticles;
    [SerializeField] public ParticleSystem miningParticles;
    [SerializeField] private Light miningLight;
    [SerializeField] public ParticleSystem starParticles;
    [SerializeField] public Slider warmingSlider;
    [SerializeField] private Image warmingFill;
    [SerializeField] private MeshRenderer meshRenderer;

    private Material windowMaterialSlot;
    private Scr_PlayerShipMovement playerShipMovement;
    private Scr_PlayerShipActions playerShipActions;

    private void Start()
    {
        playerShipMovement = GetComponent<Scr_PlayerShipMovement>();
        playerShipActions = GetComponent<Scr_PlayerShipActions>();
        windowMaterialSlot = meshRenderer.materials[5];

        warmingSlider.maxValue = playerShipMovement.warmingAmount;

        warmingSlider.gameObject.SetActive(false);
    }

    public void Update()
    {
        StarsParticles();
        WindowMaterials();
    }

    private void WindowMaterials()
    {
        if (playerShipMovement.astronautOnBoard)
            windowMaterialSlot = windowOn;

        else
            windowMaterialSlot = windowOff;
    }

    private void StarsParticles()
    {
        var emission = starParticles.emission;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed || playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landing)
        {
            emission.rateOverTime = inPlanetEmission;
            starParticles.gameObject.transform.localScale = Vector3.Lerp(starParticles.gameObject.transform.localScale, inPlanetSize, Time.deltaTime);
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.inSpace)
        {
            emission.rateOverTime = inSpaceEmission;
            starParticles.gameObject.transform.localScale = Vector3.Lerp(starParticles.gameObject.transform.localScale, inSpaceSize, Time.deltaTime);
        }
    }

    public void WarmingSliderColor()
    {
        float colorChangeSpeed = 2;

        if (warmingSlider.value >= (0.75f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color75, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.75f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color50, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.50f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color25, Time.deltaTime * colorChangeSpeed);
        if (warmingSlider.value <= (0.25f * warmingSlider.maxValue))
            warmingFill.color = Color.Lerp(warmingFill.color, color0, Time.deltaTime * colorChangeSpeed);
    }

    public void OnGroundEffects()
    {
        ParticleSystem landedDustParticles1 = GameObject.Find("LandedDustParticles1").GetComponent<ParticleSystem>();
        ParticleSystem landedDustParticles2 = GameObject.Find("LandedDustParticles2").GetComponent<ParticleSystem>();

        var thrusterEmission = thrusterParticles.emission;
        var LandedDustParticles1Emission = landedDustParticles1.emission;
        var LandedDustParticles2Emission = landedDustParticles2.emission;

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.landed)
        {
            if (!thrusterParticles.isPlaying)
                thrusterParticles.Play();
            if (!landedDustParticles1.isPlaying)
                landedDustParticles1.Play();
            if (!landedDustParticles2.isPlaying)
                landedDustParticles2.Play();

            thrusterEmission.rateOverTime = warmingSlider.value * landedThrusterMult;
            LandedDustParticles1Emission.rateOverTime = warmingSlider.value * landedDustMult;
            LandedDustParticles2Emission.rateOverTime = warmingSlider.value * landedDustMult;
        }

        if (playerShipMovement.playerShipState == Scr_PlayerShipMovement.PlayerShipState.takingOff)
        {
            thrusterEmission.rateOverTime = takingOffThrusterPower;

            landedDustParticles1.Stop();
            landedDustParticles2.Stop();
        }
    }

    public void InSpaceEffects()
    {
        var thrusterEmission = thrusterParticles.emission;

        thrusterEmission.rateOverTime = inSpaceThrusterPower;

        if (GetComponent<Scr_PlayerShipStats>().currentFuel <= 0)
            thrusterEmission.rateOverTime = 0;
    }

    public void TakingOffEffects(bool play)
    {
        if (play)
        {
            RaycastHit2D takingOffHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, playerShipMovement.planetLayer);

            if (takingOffHit)
            {
                float dustPower = Vector2.Distance(endOfShip.position, takingOffHit.point);

                if (GameObject.Find("DustParticles(Clone)") == null)
                    Instantiate(dustParticles, takingOffHit.point, gameObject.transform.rotation);

                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                if (playerShipMovement.currentPlanet != null)
                    currentDustParticles.transform.SetParent(playerShipMovement.currentPlanet.transform);

                if (!dustParticles1.isPlaying)
                    dustParticles1.Play();

                if (!dustParticles2.isPlaying)
                    dustParticles2.Play();

                if (!thrusterParticles.isPlaying)
                    thrusterParticles.Play();

                if (!takingOffDustParticles.isPlaying)
                    takingOffDustParticles.Play();

                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;
                var emission3 = thrusterParticles.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission3.rateOverTime = takingOffThrusterPower;
            }
        }

        else
        {
            GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

            thrusterParticles.Stop();
            takingOffDustParticles.Stop();

            Destroy(currentDustParticles);
        }
    }

    float dustPower = 0;

    public void LandingEffects(bool play)
    {
        if (play)
        {
            RaycastHit2D landingHit = Physics2D.Raycast(endOfShip.position, -endOfShip.up, Mathf.Infinity, playerShipMovement.planetLayer);

            if (landingHit)
            {
                dustPower = Vector2.Distance(endOfShip.position, landingHit.point);

                if (GameObject.Find("DustParticles(Clone)") == null)
                    Instantiate(dustParticles, landingHit.point, gameObject.transform.rotation);

                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");

                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                currentDustParticles.transform.SetParent(playerShipMovement.currentPlanet.transform);
                currentDustParticles.transform.position = landingHit.point;

                Vector2 direction = new Vector2(landingHit.point.x - playerShipMovement.currentPlanet.transform.position.x, landingHit.point.y - playerShipMovement.currentPlanet.transform.position.y);
                currentDustParticles.transform.up = direction;

                if (!dustParticles1.isPlaying)
                    dustParticles1.Play();

                if (!dustParticles2.isPlaying)
                    dustParticles2.Play();

                if (!thrusterParticles.isPlaying)
                    thrusterParticles.Play();

                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;
                var emission3 = thrusterParticles.emission;

                emission1.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission2.rateOverTime = (2.5f - dustPower) * dustMultiplier;
                emission3.rateOverTime = landingThrusterPower;
            }
        }

        else
        {
            if (GameObject.Find("DustParticles(Clone)") != null)
            {
                GameObject currentDustParticles = GameObject.Find("DustParticles(Clone)");
                ParticleSystem dustParticles1 = currentDustParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem dustParticles2 = currentDustParticles.transform.GetChild(1).GetComponent<ParticleSystem>();

                float newEmission;
                var emission1 = dustParticles1.emission;
                var emission2 = dustParticles2.emission;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    newEmission = Mathf.Lerp(dustPower, 0, Time.deltaTime);
                    emission1.rateOverTime = newEmission;
                    emission2.rateOverTime = newEmission;
                }

                if (playerShipMovement.landing)
                    thrusterParticles.Stop();
            }
        }
    }

    public void AttachedEffects(bool play)
    {
        if (play)
        {
            if (!thrusterParticles.isPlaying)
                thrusterParticles.Play();

            var emission = thrusterParticles.emission;

            emission.rateOverTime = attachedThrusterPower;
        }

        else
            thrusterParticles.Stop();
    }

    public void MiningEffects(bool play)
    {
        if (play)
        {
            miningLight.enabled = true;

            if (!miningParticles.isPlaying)
                miningParticles.Play();

            Vector2 direction = new Vector2(playerShipActions.laserHitPosition.x - playerShipActions.currentAsteroid.transform.position.x, playerShipActions.laserHitPosition.y - playerShipActions.currentAsteroid.transform.position.y);

            miningParticles.transform.up = direction;
            miningParticles.transform.position = playerShipActions.laserHitPosition;
        }

        else
        {
            miningLight.enabled = false;

            if (miningParticles.isPlaying)
                miningParticles.Stop();
        }
    }

    public void DamageParticleSet(bool damage)
    {
        if (damage && !damageParticles.isPlaying)
            damageParticles.Play();

        else if (damageParticles.isPlaying)
            damageParticles.Stop();
    }*/ // OLD SCRIPT
}