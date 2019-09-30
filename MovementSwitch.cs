using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using VRTK;

public class MovementSwitch : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;
    public GameObject touchpadRotate;
    public bool teleport = false;
    public GameObject motionBlurControl;
    public GameObject playArea;
    public GameObject portalScript;
    private VRTouchpadRotate rotate;
    private VRTK_TouchpadControl move;
    private MotionBlurControl blur;
    private VRTK_Pointer pointer;
    private VRTK_BezierPointerRenderer bezier;
    private WeaponSelector weaponselector;
    private VRTK_ControllerEvents controller;
    private RedirectedPortals pScript;
    
    
    void Start()
    {
        rotate = touchpadRotate.GetComponent<VRTouchpadRotate>();
        move = leftController.GetComponent<VRTK_TouchpadControl>();
        blur = motionBlurControl.GetComponent<MotionBlurControl>();
        pointer = leftController.GetComponent<VRTK_Pointer>();
        bezier = leftController.GetComponent<VRTK_BezierPointerRenderer>();
        controller = leftController.GetComponent<VRTK_ControllerEvents>();
        pScript = portalScript.GetComponent<RedirectedPortals>();
    }

    private void Update()
    {
        controller.ButtonTwoPressed += DoButtonTwoPressed;
        rotate.enabled = !teleport;
        move.enabled = !teleport;
        blur.toogleOff = teleport;
        pointer.enabled = teleport;
        bezier.enabled = teleport;
        if (!teleport)
        {
            pScript.resetPortal();
        }
    }
    
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        teleport = !teleport;
    }
    
}