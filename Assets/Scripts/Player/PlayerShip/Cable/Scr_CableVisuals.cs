using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CableVisuals : MonoBehaviour
{
    [Header("References: Distance Joints")]
    [SerializeField] private DistanceJoint2D distanceJoint1;
    [SerializeField] private DistanceJoint2D distanceJoint2;
    [SerializeField] private DistanceJoint2D distanceJoint3;
    [SerializeField] private DistanceJoint2D distanceJoint4;
    [SerializeField] private DistanceJoint2D distanceJoint5;
    [SerializeField] private DistanceJoint2D distanceJoint6;
    [SerializeField] private DistanceJoint2D distanceJoint7;
    [SerializeField] private DistanceJoint2D distanceJoint8;
    [SerializeField] private DistanceJoint2D distanceJoint9;
    [SerializeField] private DistanceJoint2D distanceJoint10;
    [SerializeField] private DistanceJoint2D distanceJoint11;
    [SerializeField] private DistanceJoint2D distanceJoint12;
    [SerializeField] private DistanceJoint2D distanceJoint13;
    [SerializeField] private DistanceJoint2D distanceJoint14;
    [SerializeField] private DistanceJoint2D distanceJoint15;

    [Header("References: Line Renderer")]
    [SerializeField] private LineRenderer cableVisuals;

    [HideInInspector] public bool printCable;

    private void Update()
    {
        if (printCable)
            PrintCableVisuals();
    }

    private void PrintCableVisuals()
    {
        cableVisuals.SetPosition(0, distanceJoint1.transform.position);
        cableVisuals.SetPosition(1, distanceJoint2.connectedBody.transform.position);
        cableVisuals.SetPosition(2, distanceJoint3.connectedBody.transform.position);
        cableVisuals.SetPosition(3, distanceJoint4.connectedBody.transform.position);
        cableVisuals.SetPosition(4, distanceJoint5.connectedBody.transform.position);
        cableVisuals.SetPosition(5, distanceJoint6.connectedBody.transform.position);
        cableVisuals.SetPosition(6, distanceJoint7.connectedBody.transform.position);
        cableVisuals.SetPosition(7, distanceJoint8.connectedBody.transform.position);
        cableVisuals.SetPosition(8, distanceJoint9.connectedBody.transform.position);
        cableVisuals.SetPosition(9, distanceJoint10.connectedBody.transform.position);
        cableVisuals.SetPosition(10, distanceJoint11.connectedBody.transform.position);
        cableVisuals.SetPosition(11, distanceJoint12.connectedBody.transform.position);
        cableVisuals.SetPosition(12, distanceJoint13.connectedBody.transform.position);
        cableVisuals.SetPosition(13, distanceJoint14.connectedBody.transform.position);
        cableVisuals.SetPosition(14, distanceJoint15.connectedBody.transform.position);
    }
}