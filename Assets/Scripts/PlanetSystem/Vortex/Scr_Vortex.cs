using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Vortex : MonoBehaviour
{
    [Header("Vortex Paramaters")]
    [SerializeField] float vortexSize;

    [Header("Size 1 Vortex Paramaters")]
    [SerializeField] float range;
    [SerializeField] float force;
    [SerializeField] float particleAmount;
    [SerializeField] float lifeTime;

    private GameObject playerShip;
    private ParticleSystem vortexParticles;
    private CircleCollider2D vortexCollider;

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");

        vortexCollider = GetComponent<CircleCollider2D>();
        vortexParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        UpdateParameters();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            Vector3 direction = new Vector3(playerShip.transform.position.x - transform.position.x, playerShip.transform.position.y - transform.position.y, playerShip.transform.position.z - transform.position.z);

            playerShip.GetComponent<Rigidbody2D>().AddForce(-direction);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range * vortexSize);
    }
}