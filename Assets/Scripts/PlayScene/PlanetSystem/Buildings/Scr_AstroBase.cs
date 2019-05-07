using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scr_AstroBase : MonoBehaviour
{
    [HideInInspector] public bool switchGravity;

    public abstract void FixedUpdate();

    public abstract Vector3 GetFutureDisplacement(Vector3 position, float time);

    public abstract Vector3 GetFutureGravity(Vector3 position, float time);
}
