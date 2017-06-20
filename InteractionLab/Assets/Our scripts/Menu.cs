using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Menu : MonoBehaviour
{

    private enum GrabMethods { NEAR_SIMPLE, NEAR_STICK, NEAR_DIST, FAR_CONTROLLER, FAR_HMD, FAR_GOGO, NONE };
    private GrabMethods choosenInteraction = GrabMethods.NONE; //(nothing is picked)
    //scripts for activating grabbing methods
    private ControllerGrabObject scriptNearSimple;
    private RaycastingMethode scriptFarController;
    private ControllerGrabViaRod scriptNearRod; //EINBINDEN
    private ControllerGrabLightlyDistance scriptNearDist; //EINBINDEN

    public bool teaching;
    public bool snap;
    public bool showingTask;
    public bool menuVisible;
    public bool activateSwitchRoom;

    //new textures for showing on or off
    public Sprite ONTeachSprite;
    public Sprite OFFTeachSprite;
    public Sprite ONSnappingSprite;
    public Sprite OFFSnappingSprite;
    public Sprite ONTaskSprite;
    public Sprite OFFTaskSprite;

    private VRTK_ControllerEvents controllerTracked;
    private RadialMenu menu1;
    private RadialMenu menu2;
    private int counter = 0;

    private GameObject switchScene;

    // Use this for initialization
    void Start()
    {
        menu1 = GameObject.Find("Panel1").GetComponent<RadialMenu>();
        menu2 = GameObject.Find("Panel2").GetComponent<RadialMenu>();
        menu1.generateOnAwake = true;
        menu2.generateOnAwake = false;

        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.Log("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");

        }

        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoReleased += new ControllerInteractionEventHandler(DoButtonTwoReleased);
        
        teaching = true;
        snap = true;
        showingTask = true;
        menuVisible = false;
        activateSwitchRoom = false;
        menu1.gameObject.SetActive(menuVisible);
        menu2.gameObject.SetActive(menuVisible);

        switchScene = GameObject.Find("switchScene");
        switchScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {  
        //Raycast has to be loaded on the start of the scene. when done: Ray invisible
        if (RaycastingMethode.StartIsReady && counter == 0)
        {
            GetScripts();
            RaycastingMethode.deleteRay();
            scriptFarController.enabled = false;
            counter++;
        }
    }

    //Get all interaction scripts
    void GetScripts()
    {
        scriptFarController = GameObject.Find("Controller (right)").GetComponent<RaycastingMethode>();
        scriptNearSimple = GameObject.Find("Controller (right)").GetComponent<ControllerGrabObject>();
    }


    //Listener for Buttons?
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "BUTTON TWO", "pressed down", e);
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        toggleMenue();
        DebugLogger(e.controllerIndex, "BUTTON TWO", "released", e);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    //Switchen menu on and off
    private void toggleMenue()
    {
        menu2.gameObject.SetActive(false);
        menuVisible = !menuVisible;
        menu1.gameObject.SetActive(menuVisible);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------- FUNCTIONS FOR BUTTONS -------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void switchMenu()
    {
        //GetScripts();
        menu1.gameObject.SetActive(!menu1.gameObject.activeSelf);
        menu2.gameObject.SetActive(!menu2.gameObject.activeSelf);
    }

    public void activateNearSimple()
    {
        RaycastingMethode.deleteRay();
        choosenInteraction = GrabMethods.NEAR_SIMPLE;
        scriptNearSimple.enabled = true;
        scriptFarController.enabled = false;
        toggleMenue();
    }

    public void activateNearStick()
    {
        choosenInteraction = GrabMethods.NEAR_STICK;
    }

    public void activateNearDist()
    {
        choosenInteraction = GrabMethods.NEAR_DIST;
    }

    public void activateFarController()
    {
        //choosenInteraction = GrabMethods.FAR_CONTROLLER;
        scriptFarController.enabled = true;
        scriptNearSimple.enabled = false;
        RaycastingMethode.ActivateRay();
        toggleMenue();
    }

    public void activateFarHMD()
    {
        choosenInteraction = GrabMethods.FAR_HMD;
    }

    public void activateFarGOGO()
    {
        choosenInteraction = GrabMethods.FAR_GOGO;
    }

    public void enableTeaching()
    {
        teaching = !teaching;

        if (teaching)
        {
            menu1.buttons[2].ButtonIcon = ONTeachSprite;
        }
        else
        {
            menu1.buttons[2].ButtonIcon = OFFTeachSprite;
        }
        menu1.RegenerateButtons();
    }

    public void reset()
    {
        //Reload scene? 
    }

    public void enableSnapping()
    {
        snap = !snap;

        if (snap)
        {
            menu1.buttons[1].ButtonIcon = ONSnappingSprite;
        }
        else
        {
            menu1.buttons[1].ButtonIcon = OFFSnappingSprite;
        }
        menu1.RegenerateButtons();
    }

    public void startMesure()
    {
        TargetTest t = GameObject.Find("TargetObject").GetComponent<TargetTest>();
        t.startGrabTime();

    }

    public void enableShowingTask()
    {
        showingTask = !showingTask;

        if (showingTask)
        {
            menu1.buttons[2].ButtonIcon = ONTaskSprite;
        }
        else
        {
            menu1.buttons[2].ButtonIcon = OFFTaskSprite;
        }
        menu1.RegenerateButtons();
    }

    public void switchRoom()
    {
        activateSwitchRoom = true;
        switchScene.SetActive(true);
    }

}
