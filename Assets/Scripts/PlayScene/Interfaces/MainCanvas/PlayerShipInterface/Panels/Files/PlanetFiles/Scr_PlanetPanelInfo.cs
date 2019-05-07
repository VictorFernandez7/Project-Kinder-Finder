using UnityEngine.UI;
using UnityEngine;

public class Scr_PlanetPanelInfo : MonoBehaviour
{
    [Header("Planet Information")]
    [SerializeField] public string planetName;
    [SerializeField] public bool highTemp;
    [SerializeField] public bool lowTemp;
    [SerializeField] public bool toxic;
    [SerializeField] public bool jetpack;
    [SerializeField] public Sprite res1;
    [SerializeField] public Sprite res2;
    [SerializeField] public Sprite res3;
    [SerializeField] public Sprite res4;
    [SerializeField] public Sprite res5;
    [TextArea] [SerializeField] public string history;
}