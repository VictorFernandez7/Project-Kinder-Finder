using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scr_ToolBase : MonoBehaviour
{
    public new string name;

    public abstract void Update();

    public abstract void UseTool();

    public abstract void Function();
}
