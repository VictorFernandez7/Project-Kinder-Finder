using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scr_ToolBase : MonoBehaviour
{
    public string toolName;

    [HideInInspector] public GameObject resource;
    [HideInInspector] public int resourceAmount;
    [HideInInspector] public bool onHands;
    [HideInInspector] public int resourceLeft;

    public abstract void Update();

    public abstract void UseTool();

    public abstract void Function();

    public abstract void RecoverTool();

    public abstract void OnMouseEnter();

    public abstract void OnMouseExit();
}
