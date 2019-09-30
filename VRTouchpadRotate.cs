using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

//-------------------------------------------------------------------------------------------------\\
// This code has been developed by Feisty Crab Studios for personal, commercial, and education use.\\
//                                                                                                 \\
// You are free to edit and redistribute this code, subject to the following:                      \\
//                                                                                                 \\
//      1. You will not sell this code or an edited version of it.                                 \\
//      2. You will not remove the copyright messages                                              \\
//      3. You will give credit to Feisty Crab Studios if used commercially                        \\
//      4. Don't be a mean sausage, nobody likes a mean sausage.                                   \\
//                                                                                                 \\
// Contact us @ feistycrabstudios.gmail.com with any questions.                                    \\
//-------------------------------------------------------------------------------------------------\\

public class VRTouchpadRotate : MonoBehaviour
{

    [SerializeField]
    private Transform rig;

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    private Vector2 axis = Vector2.zero;

    private float _mMoveSpeed = 2.5f;
    private float _mHorizontalTurnSpeed = 22.5f;
    private bool _mInverted = false;
    private const float VERTICAL_LIMIT = 60f;

    private float dt = 0.25f;
    private float timer;
    private float timer2;
    private bool snap;
    private bool fade;
    private bool faded;
    private bool activateFade;

    public VRTK_HeadsetFade headsetFade;

    private float previousTurn;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        previousTurn = 0;
        timer = 0f;
        timer2 = 0f;
        snap = false;
        fade = false;
        faded = false;
        activateFade = false;
    }

    float GetAngle(float input)
    {
        if (input < 0f)
        {
            return -Mathf.LerpAngle(0, VERTICAL_LIMIT, -input);
        }
        else if (input > 0f)
        {
            return Mathf.LerpAngle(0, VERTICAL_LIMIT, input);
        }
        return 0f;
    }

    protected virtual void StartFade()
    {
        headsetFade.Fade(Color.black, 0.05f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        var device = SteamVR_Controller.Input((int)trackedObj.index);

        if (activateFade)
        {
            if (!fade)
            {
                Invoke("StartFade", 0f);
                fade = true;
                timer2 = 0f;
            }
            else if (timer2 >= 0.45f)
            {
                fade = false;
                CancelInvoke("StartFade");
                headsetFade.Unfade(0.05f);
                activateFade = false;
            }
        }

        if (controller.GetTouch(touchpad))
        {
            var touchPadVector = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            Quaternion orientation = rig.transform.rotation;

            if (rig != null && !faded)
            {
                //float dTurn;
                //dTurn = rig.rotation.y - previousTurn;
                //previousTurn = rig.rotation.y;
                //float angularSpeed = dTurn/Time.deltaTime;
                //float x = touchPadVector.x;
                //x = Mathf.Pow(x, 5);
                //if (x < -0.4 && !snap)
                //{
                //    rig.transform.Rotate(0, -22.5f, 0, Space.World);
                //    snap = true;
                //}
                //else if (x > 0.4 && !snap)
                //{
                //    rig.transform.Rotate(0, 22.5f, 0, Space.World);
                //    snap = true;
                //}
                //else
                //{
                //    rig.transform.Rotate(0, _mHorizontalTurnSpeed * Time.deltaTime * x * 10, 0, Space.World);
                //    snap = false;
                //}
                //if (snap && !fade)
                //{
                //    Invoke("StartFade", 0f);
                //    fade = true;
                //    timer2 = 0f;

                //}
                //else if (timer2 >= 0.4f && fade)
                //{
                //    fade = false;
                //    CancelInvoke("StartFade");
                //    headsetFade.Unfade(0.05f);
                //    faded = true;
                //}

                float rotateSpeed = Mathf.Sign(touchPadVector.x) * (10f / (1f-Mathf.Exp(3f * Mathf.Abs(touchPadVector.x) + Mathf.Log((10f / 22.5f) + 1f))) + 22.5f);
                rig.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);
            }
            
        }

        if (controller.GetPressDown(touchpad))
        {
            var touchPadVector = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            activateFade = true;
            rig.transform.Rotate(0, 22.5f*Mathf.Sign(touchPadVector.x), 0, Space.World);
        }

        //if (controller.GetTouchUp(touchpad))
        //{
        //    faded = false;
        //    if (fade)
        //    {
        //        fade = false;
        //        CancelInvoke("StartFade");
        //        headsetFade.Unfade(0);
        //    }
        //}
    }
}