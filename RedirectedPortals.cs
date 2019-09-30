using System;
using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

public class RedirectedPortals : MonoBehaviour
{

    public GameObject cameraRig;
    public GameObject eyeObject;
    public GameObject portal;
    public float cooldown = 5f;
    private Transform port;
    private Transform cam;
    private Transform eye;
    private Vector3 Vpc;
    private Vector3 Vup;
    private Vector3 Vpd;
    private float phi;
    private float gamma;
    public float alpha = 40f;
    public float beta = 10f;
    public float depth = 3f;
    private float direction;
    private Vector3 Vport;
    private float timer;

    // Use this for initialization
    void Start ()
    {
        cam = cameraRig.GetComponent<Transform>();
        eye = eyeObject.GetComponent<Transform>();
        port = portal.GetComponent<Transform>();
        Vpc = new Vector3();
        Vup = new Vector3();
        Vpd = new Vector3();
        phi = 0f;
        gamma = 0f;
        timer = 0f;
    }
	
	
    public void MakePortal ()
    {
        Vpc = (eye.position - cam.position);
        Vup = eye.up;
        Vpd = eye.forward;
        Vpc.y = 0;
        Vpd.y = 0;
        Vpc = Vpc.normalized;
        Vpd = Vpd.normalized;
        phi = Vector3.Angle(Vpd, Vpc);


        if (Math.Round(Vector3.Cross(Vpc, Vpd).y) == 0f)
        {
            if (Vector3.Magnitude(Vpc - Vpd) < 0.1)
            {
                alpha = 0;
            }
            else
            {
                alpha = alpha;
            }
        }
        else
        {
            if ((Vector3.Dot(Vector3.Cross(Vpc, Vpd), Vup)) > 0)
            {
                alpha = 360 - alpha;
            }
            else
            {
                alpha = alpha;
            }
        }

        if (phi >= alpha + beta)
        {
            gamma = alpha + beta;
        }
        else if (phi < alpha)
        {
            gamma = alpha;
            phi = alpha;
            beta = 0;
        }
        else if (phi > alpha)
        {
            gamma = alpha;
            beta = phi - alpha;
        }
        else if (phi == alpha)
        {
            gamma = phi;
            beta = 0;
        }
        Vport = new Vector3(Vpd.x * Mathf.Cos(alpha * Mathf.Deg2Rad) - Vpd.z * Mathf.Sin(alpha * Mathf.Deg2Rad), 0, Vpd.x * Mathf.Sin(alpha * Mathf.Deg2Rad) + Vpd.z * Mathf.Cos(alpha * Mathf.Deg2Rad));
        port.position = eye.position + depth * Vport;
        port.LookAt(eye.position, transform.up);
        port.Rotate(new Vector3(0, beta, 0), Space.Self);

    }

    public void resetPortal()
    {
        port.position = Vector3.zero;
    }

}