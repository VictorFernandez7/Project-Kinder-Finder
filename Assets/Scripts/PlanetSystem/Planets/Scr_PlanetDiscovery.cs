using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlanetDiscovery : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject planet;

    [HideInInspector] public bool sighted;
    [HideInInspector] public bool explored;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        circleCollider.enabled = !sighted;
        planet.SetActive(sighted);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerShip"))
            sighted = true;
    }

    public void PlanetExplored()
    {
        explored = true;
    }
}