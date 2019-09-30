using UnityEngine;
using System;
using VRTK;

public class Controller : MonoBehaviour
{
    private VRTK_ControllerEvents controllerEvents;
    private void OnEnable()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }
        controllerEvents.TriggerPressed += DoTriggerPressed;
        controllerEvents.TriggerReleased += DoTriggerReleased;
    }

    private void OnDisable()
    {
        if (controllerEvents != null)
        {
            controllerEvents.TriggerPressed -= DoTriggerPressed;
            controllerEvents.TriggerReleased -= DoTriggerReleased;
        }
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
    }
}
