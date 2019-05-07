using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Breakeable : MonoBehaviour
{
    public float amount;

    private void Update()
    {
        if (amount <= 0)
            Destroy(this.gameObject);
    }
}
