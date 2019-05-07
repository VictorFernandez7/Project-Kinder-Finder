using UnityEngine.UI;
using UnityEngine;

public class Scr_SpriteCheck : MonoBehaviour
{
    private Image sprite;

    private void Start()
    {
        sprite = GetComponent<Image>();
    }

    private void Update()
    {
        if (sprite == null)
            gameObject.SetActive(false);
    }
}