using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Vortex : MonoBehaviour
{
    [Header("Vortex Paramaters")]
    [SerializeField] float creationDelay;
    [SerializeField] float vortexSize;
    [SerializeField] float dyingSpeed;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    [Header("Size 1 Vortex Paramaters")]
    [SerializeField] float range;
    [SerializeField] float force;
    [SerializeField] float particleAmount;
    [SerializeField] float lifeTime;

    [Header("References")]
    [SerializeField] private GameObject playerShip;

    private bool checkDestroy = true;
    private float checkDestroyTime = 0.25f;
    private ParticleSystem vortexParticles;
    private CircleCollider2D vortexCollider;

    private void Start()
    {
        vortexCollider = GetComponent<CircleCollider2D>();
        vortexParticles = GetComponentInChildren<ParticleSystem>();

        vortexSize = Random.Range(minSize, maxSize);
    }

    private void Update()
    {
        UpdateParameters();
        DyingProcess();

        checkDestroyTime -= Time.deltaTime;
        creationDelay -= Time.deltaTime;

        if (checkDestroyTime <= 0)
            checkDestroy = false;

        if (creationDelay <= 0)
            vortexParticles.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            Vector3 direction = new Vector3(playerShip.transform.position.x - transform.position.x, playerShip.transform.position.y - transform.position.y, playerShip.transform.position.z - transform.position.z);

            playerShip.GetComponent<Rigidbody2D>().AddForce(-direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkDestroy)
        {
            if (!collision.gameObject.CompareTag("PlayerShip"))
                Destroy(gameObject);
        }
    }

    private void UpdateParameters()
    {
        var main = vortexParticles.main;
        var emission = vortexParticles.emission;

        main.startLifetime = lifeTime * vortexSize;
        emission.rateOverTime = particleAmount * vortexSize;
        vortexCollider.radius = (range * 3) * vortexSize;
    }

    private void DyingProcess()
    {
        if (vortexSize > 0)
            vortexSize -= dyingSpeed * Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range * vortexSize);
    }
}