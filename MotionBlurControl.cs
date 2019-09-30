using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Rendering.PostProcessing;
using VRTK;

public class MotionBlurControl : MonoBehaviour {
    
    public GameObject camera;
    public GameObject controlObject;
    public float minimumSpeed = 3;
    public GameObject blur;
    public bool toogleOff;
    private VRTK_TouchpadControl control;
    private Transform camPos;
    private Vector2 previousXY;
    private Vector2 currentXY;
    private float timer;

    // Use this for initialization
    void Start ()
    {
        camPos = camera.GetComponent<Transform>();
        control = controlObject.GetComponent<VRTK_TouchpadControl>();
        previousXY = new Vector2(camPos.position.x, camPos.position.z);
        timer = 0f;
        blur.SetActive(false);
    }
	
    // Update is called once per frame
    void Update ()
    {
        if (!toogleOff)
        {
            timer += Time.deltaTime;
            currentXY = new Vector2(camPos.position.x, camPos.position.z);
            if (Vector2.Distance(previousXY, currentXY) / Time.deltaTime > minimumSpeed && control.enabled)
            {
                blur.SetActive(true);
            }
            else if (timer > 1)
            {
                timer = 0f;
                blur.SetActive(false);
            }
            previousXY = currentXY;
        }
        else
        {
            blur.SetActive(false);
        }
    }
}