using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Orbit : MonoBehaviour {

    [SerializeField] private LineRenderer orbitLine;
    [SerializeField] private Transform planet;
    [SerializeField] private Transform pivot;

    void Start () {
        float magnitude;
        int index = 0;
        magnitude = (planet.position - pivot.position).magnitude;
        orbitLine.positionCount = 41;
        
        for(float i = 1; i >= 0; i -= 0.1f)
        {
            Vector2 vectorDirector = new Vector2(i, 1 - i);
            orbitLine.SetPosition(index, (vectorDirector * magnitude));
            index += 1;
        }

        for(float i = 0; i >= -1; i -= 0.1f)
        {
            Vector2 vectorDirector = new Vector2(i, 1 + i);
            orbitLine.SetPosition(index, (vectorDirector * magnitude));
            index += 1;
        }

        for(float i = -1; i <= 0; i += 0.1f)
        {
            Vector2 vectorDirector = new Vector2(i, - 1 - i);
            orbitLine.SetPosition(index, (vectorDirector * magnitude));
            index += 1;
        }

        for(float i = 0; i <= 1; i += 0.1f)
        {
            Vector2 vectorDirector = new Vector2(i, -1 + i);
            orbitLine.SetPosition(index, (vectorDirector * magnitude));
            index += 1;
        }

        orbitLine.SetPosition(40, (new Vector2(1, 0) * magnitude));
    }
	
}
