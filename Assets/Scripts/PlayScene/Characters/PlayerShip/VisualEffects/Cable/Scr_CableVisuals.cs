using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CableVisuals : MonoBehaviour
{
    [Header("References: Distance Joints")]
    [SerializeField] private DistanceJoint2D cableStart;
    [SerializeField] private DistanceJoint2D cable1;
    [SerializeField] private DistanceJoint2D cable2;
    [SerializeField] private DistanceJoint2D cable3;
    [SerializeField] private DistanceJoint2D cable4;
    [SerializeField] private DistanceJoint2D cable5;
    [SerializeField] private DistanceJoint2D cable6;
    [SerializeField] private DistanceJoint2D cable7;
    [SerializeField] private DistanceJoint2D cable8;
    [SerializeField] private DistanceJoint2D cable9;
    [SerializeField] private DistanceJoint2D cable10;
    [SerializeField] private DistanceJoint2D cable11;
    [SerializeField] private DistanceJoint2D cable12;
    [SerializeField] private DistanceJoint2D cable13;
    [SerializeField] private DistanceJoint2D cable14;
    [SerializeField] private DistanceJoint2D cable15;
    [SerializeField] private DistanceJoint2D cable16;
    [SerializeField] private DistanceJoint2D cable17;
    [SerializeField] private DistanceJoint2D cable18;
    [SerializeField] private DistanceJoint2D cable19;
    [SerializeField] private DistanceJoint2D cable20;
    [SerializeField] private DistanceJoint2D cable21;
    [SerializeField] private DistanceJoint2D cable22;
    [SerializeField] private DistanceJoint2D cable23;
    [SerializeField] private DistanceJoint2D cable24;
    [SerializeField] private DistanceJoint2D cable25;
    [SerializeField] private DistanceJoint2D cable26;
    [SerializeField] private DistanceJoint2D cable27;
    [SerializeField] private DistanceJoint2D cable28;
    [SerializeField] private DistanceJoint2D cableEnd;

    [Header("References: Line Renderer")]
    [SerializeField] private LineRenderer cableVisuals;

    [HideInInspector] public bool printCable;

    private void Update()
    {
        if (printCable)
        {
            cableVisuals.enabled = true;

            cable1.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable2.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable3.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable4.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable5.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable6.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable7.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable8.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable9.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable10.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable11.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable12.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable13.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable14.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable15.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable16.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable17.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable18.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable19.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable20.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable21.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable22.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable23.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable24.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable25.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable26.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable27.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cable28.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            cableEnd.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

            cableVisuals.SetPosition(0, cableStart.transform.position);
            cableVisuals.SetPosition(1, cableStart.connectedBody.transform.position);
            cableVisuals.SetPosition(2, cable1.connectedBody.transform.position);
            cableVisuals.SetPosition(3, cable2.connectedBody.transform.position);
            cableVisuals.SetPosition(4, cable3.connectedBody.transform.position);
            cableVisuals.SetPosition(5, cable4.connectedBody.transform.position);
            cableVisuals.SetPosition(6, cable5.connectedBody.transform.position);
            cableVisuals.SetPosition(7, cable6.connectedBody.transform.position);
            cableVisuals.SetPosition(8, cable7.connectedBody.transform.position);
            cableVisuals.SetPosition(9, cable8.connectedBody.transform.position);
            cableVisuals.SetPosition(10, cable9.connectedBody.transform.position);
            cableVisuals.SetPosition(11, cable10.connectedBody.transform.position);
            cableVisuals.SetPosition(12, cable11.connectedBody.transform.position);
            cableVisuals.SetPosition(13, cable12.connectedBody.transform.position);
            cableVisuals.SetPosition(14, cable13.connectedBody.transform.position);
            cableVisuals.SetPosition(15, cable14.connectedBody.transform.position);
            cableVisuals.SetPosition(16, cable15.connectedBody.transform.position);
            cableVisuals.SetPosition(17, cable16.connectedBody.transform.position);
            cableVisuals.SetPosition(18, cable17.connectedBody.transform.position);
            cableVisuals.SetPosition(19, cable18.connectedBody.transform.position);
            cableVisuals.SetPosition(20, cable19.connectedBody.transform.position);
            cableVisuals.SetPosition(21, cable20.connectedBody.transform.position);
            cableVisuals.SetPosition(22, cable21.connectedBody.transform.position);
            cableVisuals.SetPosition(23, cable22.connectedBody.transform.position);
            cableVisuals.SetPosition(24, cable23.connectedBody.transform.position);
            cableVisuals.SetPosition(25, cable24.connectedBody.transform.position);
            cableVisuals.SetPosition(26, cable25.connectedBody.transform.position);
            cableVisuals.SetPosition(27, cable26.connectedBody.transform.position);
            cableVisuals.SetPosition(28, cable27.connectedBody.transform.position);
            cableVisuals.SetPosition(29, cable28.connectedBody.transform.position);
            cableVisuals.SetPosition(30, cableEnd.connectedBody.transform.position);
        }

        else
        {
            cable1.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable2.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable3.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable4.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable5.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable6.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable7.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable8.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable9.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable10.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable11.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable12.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable13.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable14.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable15.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable16.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable17.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable18.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable19.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable20.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable21.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable22.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable23.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable24.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable25.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable26.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable27.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cable28.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            cableEnd.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    public void ResetCable()
    {
        cableStart.transform.localPosition = Vector3.zero;
        cable1.transform.localPosition = Vector3.zero;
        cable2.transform.localPosition = Vector3.zero;
        cable3.transform.localPosition = Vector3.zero;
        cable4.transform.localPosition = Vector3.zero;
        cable5.transform.localPosition = Vector3.zero;
        cable6.transform.localPosition = Vector3.zero;
        cable7.transform.localPosition = Vector3.zero;
        cable8.transform.localPosition = Vector3.zero;
        cable9.transform.localPosition = Vector3.zero;
        cable10.transform.localPosition = Vector3.zero;
        cable11.transform.localPosition = Vector3.zero;
        cable12.transform.localPosition = Vector3.zero;
        cable13.transform.localPosition = Vector3.zero;
        cable14.transform.localPosition = Vector3.zero;
        cable15.transform.localPosition = Vector3.zero;
        cable16.transform.localPosition = Vector3.zero;
        cable17.transform.localPosition = Vector3.zero;
        cable18.transform.localPosition = Vector3.zero;
        cable19.transform.localPosition = Vector3.zero;
        cable20.transform.localPosition = Vector3.zero;
        cable21.transform.localPosition = Vector3.zero;
        cable22.transform.localPosition = Vector3.zero;
        cable23.transform.localPosition = Vector3.zero;
        cable24.transform.localPosition = Vector3.zero;
        cable25.transform.localPosition = Vector3.zero;
        cable26.transform.localPosition = Vector3.zero;
        cable27.transform.localPosition = Vector3.zero;
        cable28.transform.localPosition = Vector3.zero;
        cableEnd.transform.localPosition = Vector3.zero;

        cableVisuals.enabled = false;
    }
}