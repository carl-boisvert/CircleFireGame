using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private Quaternion originalRotation;

    public Transform gunTip;

    private string State;

    private void Awake()
    {
        originalRotation = transform.rotation;
        lr = GetComponent<LineRenderer>();
        State = "none";
    }

    private void Update()
    {
        RunState();
    }

    private void RunState()
    {
        if (State == "none") StartIdle();

        if (State == "Idle") Idle();
        if (State == "Grapple") Grapple();
    }

    private void StartIdle()
    {
        State = "Idle";
    }

    private void Idle()
    {

    }

    private void StopIdle()
    {
        State = "none";
    }

    public void StartGrapple(Vector3 grappleP)
    {
        grapplePoint = grappleP;
        lr.positionCount = 2;
        State = "Grapple";
    }

    void Grapple()
    {
        transform.LookAt(grapplePoint);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        grapplePoint = Vector3.zero;
        transform.rotation = transform.parent.rotation;
        State = "none";
    }
}
