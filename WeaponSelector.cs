using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class WeaponSelector : MonoBehaviour {

    public VRTK_InteractableObject sword;
    public VRTK_InteractableObject wand;
    public VRTK_InteractableObject bow;
    public GameObject LController;
    public GameObject RController;
    public GameObject arrowSpawner;
    private VRTK_InteractGrab lgrab;
    private VRTK_InteractGrab rgrab;
    private GameObject weapon;
    private VRTK_ObjectAutoGrab autograb;
    private VRTK_ControllerEvents controllerevents;
    public VRTK_ControllerEvents othercontroller;
    private VRTK_InteractableObject currentWeapon;
    private MovementSwitch move;
    private Wand wandscript;
    private Sword swordscript;
    private bool isItASword;

	// Use this for initialization
	void Start () {
        lgrab = LController.GetComponent<VRTK_InteractGrab>();
        rgrab = RController.GetComponent<VRTK_InteractGrab>();
        autograb = RController.GetComponent<VRTK_ObjectAutoGrab>();
        arrowSpawner.SetActive(false);
        isItASword = true;
        swordscript = sword.GetComponent<Sword>();
        swordscript.LController = LController;
        swordscript.RController = RController;
    }
	
	// Update is called once per frame
	void Update () {
        wandscript = wand.GetComponent<Wand>();
        if (lgrab.GetGrabbedObject() != null)
        {
            weapon = lgrab.GetGrabbedObject();
            controllerevents = RController.GetComponent<VRTK_ControllerEvents>();
            wandscript.controller = LController.GetComponent<VRTK_ControllerEvents>();
            swordscript.controllerEvents = LController.GetComponent<VRTK_ControllerEvents>();
        }
        else
        {
            weapon = rgrab.GetGrabbedObject();
            controllerevents = RController.GetComponent<VRTK_ControllerEvents>();
            wandscript.controller = RController.GetComponent<VRTK_ControllerEvents>();
            swordscript.controllerEvents = RController.GetComponent<VRTK_ControllerEvents>();
        }
        if (controllerevents == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }
        controllerevents.ButtonTwoPressed += DoButtonTwoPressed;
        controllerevents.ButtonTwoReleased += DoButtonTwoReleased;
    }

    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        autograb.enabled = false;
        if (weapon.tag == "Sword")
        {
            autograb.objectToGrab = wand;
            isItASword = false;
        }
        else if (weapon.tag == "Wand")
        {
            autograb.objectToGrab = bow;
            arrowSpawner.SetActive(true);
        }
        else if (weapon.tag == "Bow")
        {
            autograb.objectToGrab = sword;
            arrowSpawner.SetActive(false);
            isItASword = true;
        }
        if (weapon != null)
        {
            if (lgrab.GetGrabbedObject() != null)
            {
                lgrab.ForceRelease();
                Destroy(weapon);
            }
            else
            {
                rgrab.ForceRelease();
                Destroy(weapon);
            }
        }
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        autograb.enabled = true;
    }
}
