using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class DynamicFocus : MonoBehaviour
{

    public GameObject camera;
    public LayerMask mask;
    
    private const float depth = 50f;
    private const float r = 75f;
    private const float d = 150f;
    private const float a = 60f;
    private const float numberOfRays = 360;
    private Transform camPos;
    private Physics physics;
    private float[] radii;
    private List<GameObject> objects;
    private List<float> RM;
    private float[] dimX;
    private float[] dimY;

    public float a1;
    public float a2;
    public float a3;
    public float w1 = 4;
    public float w2;
    public float threshold;
    public List<GameObject> pixels;

    
    
    void Start()
    {
        camPos = camera.GetComponent<Transform>();
        radii = new float[] {2f, 4f, 6f, 8f};
        dimX = new float[] {2f, 1f, 0f, -1f, -2f};
        dimY = new float[] {1f, 0f, -1f, -2f};
    }

    void FixedUpdate()
    {
        objects = new List<GameObject>();
        RM = new List<float>();
        MakeRays();
        FocusLens();

    }

    private void MakeRays()
    {
        float stepAngleDeg = 360 / numberOfRays;
        
        foreach (float j in radii)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Vector3 localRotation = camPos.right * Mathf.Cos(i * stepAngleDeg) * j + camPos.up * Mathf.Sin(i * stepAngleDeg) * j;
                Vector3 direction = (Quaternion.Euler(localRotation) * camPos.forward).normalized;
                Ray ray = new Ray(camPos.position, direction);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, depth, layerMask:mask.value))
                {
                    GameObject g = hitInfo.collider.gameObject;
                    if (!objects.Contains(g))
                    {
                        objects.Add(g);
                        RM.Add(w1/j * 1/1440);
                    }
                    else
                    {
                        RM[objects.IndexOf(g)] += w1/j * 1/1440;
                    }
                }

                //Debug.DrawLine(ray.origin, ray.origin+(direction*depth), Color.red);
            }
        }

        foreach (RaycastHit hit in ConeCastExtension.ConeCastAll(physics, camPos.position, r, camPos.forward, d, a, mask))
        {
            float extraValue = 0f;
            FocusWeight f = hit.collider.gameObject.GetComponent<FocusWeight>();
            float metricValue = 0f;
            if (f != null)
            {
                extraValue = f.weight;
            }
            if (objects.Contains(hit.collider.gameObject))
            {

                metricValue = RM[objects.IndexOf(hit.collider.gameObject)];
            }

            float value = a1 * metricValue + a2 * Mathf.Exp(-w2 * hit.distance) + a3 * extraValue;

            if (value > threshold)
            {
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                hit.collider.gameObject.layer = LayerMask.NameToLayer("Focus");
                foreach (Transform trans in hit.collider.gameObject.GetComponentsInChildren<Transform>(true)) {
                    trans.gameObject.layer = LayerMask.NameToLayer("Focus");
                }
            }
        }
//        foreach (GameObject obj in objects)
//        {
//            distances.Add(1/Vector3.Magnitude(obj.transform.position - camPos.transform.position)); 
//        }
    }

    private void FocusLens()
    {
        float offsetX = 0.2f;
        float offsetY = 0.2f;
        int count = 0;
        foreach (float j in dimY)
        {
            foreach (float i in dimX)
            {
                Ray ray = new Ray(camPos.position + new Vector3(offsetX * i, offsetY * j, 0), camPos.forward+ new Vector3(offsetX * i, offsetY * j, 0));
                RaycastHit hitInfo;
                GameObject pixel = pixels.ElementAt(count);
                if (Physics.Raycast(ray, out hitInfo, 200, layerMask:mask.value))
                {
                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Focus"))
                    {
                        pixel.SetActive(false);
                    }
                    
                }
                else
                {
                    pixel.SetActive(true);
                }

                if (count == 7)
                {
                    pixel.SetActive(false);
                }
                //Debug.DrawLine(ray.origin, ray.origin + 100 * ray.direction , Color.red);
                count += 1;
            }
        }
    }
}