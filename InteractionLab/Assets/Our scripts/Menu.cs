﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Menu : MonoBehaviour
{
    
    //scripts for activating grabbing methods
    private ControllerGrabObject scriptNearSimple;
    private ControllerGrabLightlyDistance scriptNearDist;
    private AllRaycastMethods scriptAllRays;

    public static bool teaching = false;
    public bool snap;
    public bool showingTask;
    public bool menuVisible;
    public bool activateSwitchRoom;
    public bool isLearnRoom = false;

    public static bool RodIsEnabled = false;

    private SelfTeaching selfTeaching;

    //new textures for showing on or off
    public Sprite ONTeachSprite;
    public Sprite OFFTeachSprite;
    public Sprite ONSnappingSprite;
    public Sprite OFFSnappingSprite;
    public Sprite ONTaskSprite;
    public Sprite OFFTaskSprite;

    private VRTK_ControllerEvents controllerTracked;
    private bool start;
    private RadialMenu menu0;
    private RadialMenu menu1;
    private RadialMenu menu2;
    private int counter = 0;

    private GameObject switchScene;

    private TargetTest tTest;
    private LearningTargetTest learningtTest;
    public enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };
    enum CLOSEMethod { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD };
    enum FarMethod { FAR_RAYCAST, FAR_INDIRECT_RAYD };


    private int wrapCLoseToMethod(int close)
    {
        int wrap = -1;
        switch (close)
        {
            case (int)FarMethod.FAR_RAYCAST:
                wrap = (int)Method.FAR_RAYCAST;
                break;
            case (int)FarMethod.FAR_INDIRECT_RAYD:
                wrap = (int)Method.FAR_INDIRECT_RAY;
                break;
            default:
                break;

        }
        return wrap;
    }


    // Use this for initialization
    void Start()
    {
        start = true;
        menu0 = GameObject.Find("Panel0").GetComponent<RadialMenu>();
        menu1 = GameObject.Find("Panel1").GetComponent<RadialMenu>();
        menu2 = GameObject.Find("Panel2").GetComponent<RadialMenu>();
        menu0.generateOnAwake = true;
        menu1.generateOnAwake = false;
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
        menu0.gameObject.SetActive(menuVisible);
        menu1.gameObject.SetActive(menuVisible);
        menu2.gameObject.SetActive(menuVisible);

        switchScene = GameObject.Find("switchScene");
        switchScene.SetActive(false);
        if (isLearnRoom)
        {
            learningtTest = GameObject.Find("TargetObject").GetComponent<LearningTargetTest>();

        }
        else
        {

            tTest = GameObject.Find("TargetObject").GetComponent<TargetTest>();
        }
        selfTeaching = GameObject.Find("RightController").GetComponent<SelfTeaching>();
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastingMethode.StartIsReady Raycast has to be loaded on the start of the scene. when done: Ray invisible
        if (AllRaycastMethods.StartIsReady && counter == 0)
        {
            GetScripts();
            AllRaycastMethods.deleteRay();
            scriptAllRays.enabled = false;
            counter++;
        }
    }

    //Get all interaction scripts
    void GetScripts()
    {
        scriptNearSimple = GameObject.Find("Controller (right)").GetComponent<ControllerGrabObject>();
        scriptNearDist = GameObject.Find("Controller (right)").GetComponent<ControllerGrabLightlyDistance>();
        scriptAllRays = GameObject.Find("Controller (right)").GetComponent<AllRaycastMethods>();
    }

    private void disableScripts()
    {
        AllRaycastMethods.deleteRay();
        scriptNearDist.enabled = false;
        scriptNearSimple.enabled = false;
        scriptAllRays.enabled = false;
    }
    //Listener for Buttons?
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        // DebugLogger(e.controllerIndex, "BUTTON TWO", "pressed down", e);
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        toggleMenue();

        //DebugLogger(e.controllerIndex, "BUTTON TWO", "released", e);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    //Switchen menu on and off
    private void toggleMenue()
    {
        if (start)
        {
            menu1.gameObject.SetActive(false);
            menu2.gameObject.SetActive(false);
            menuVisible = !menuVisible;
            menu0.gameObject.SetActive(menuVisible);
            if (teaching) selfTeaching.increaseCounter();
        }
        else
        {
            menu0.gameObject.SetActive(false);
            menu2.gameObject.SetActive(false);
            menuVisible = !menuVisible;
            menu1.gameObject.SetActive(menuVisible);
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------- FUNCTIONS FOR BUTTONS -------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    public void switchMenu()
    {
        //GetScripts();
        if (teaching && menu1.gameObject.activeSelf) selfTeaching.increaseCounter();
        menu1.gameObject.SetActive(!menu1.gameObject.activeSelf);
        menu2.gameObject.SetActive(!menu2.gameObject.activeSelf);

    }

    public void activateNearSimple()
    {
        if (teaching) selfTeaching.increaseCounter();
        disableScripts();
        //RaycastingMethode.deleteRay();
        //choosenInteraction = GrabMethods.NEAR_SIMPLE;
        scriptNearSimple.enabled = true;
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_SIMPLE);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_SIMPLE);
        }
        else
        {
            tTest.setMethodID((int)Method.CLOSE_SIMPLE);
        }
        toggleMenue();
    }

    public void activateNearRod()
    {
        disableScripts();

        scriptAllRays.enabled = true;
        AllRaycastMethods.caseRay = (int)Method.CLOSE_ROD;
        AllRaycastMethods.ActivateRay();
        //choosenInteraction = Method.CLOSE_ROD;
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_ROD);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_ROD);
        }
        else
        {
            tTest.setMethodID((int)Method.CLOSE_ROD);
        }
        toggleMenue();
    }

    public void activateNearDist()
    {
        disableScripts();
        scriptNearDist.enabled = true;
        //choosenInteraction = Method.CLOSE_DIST;
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_DIST);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_DIST);
        }
        else
        {
            tTest.setMethodID((int)Method.CLOSE_DIST);
        }
        toggleMenue();
    }

    public void activateFarController()
    {
        if (teaching) selfTeaching.increaseCounter();
        disableScripts();
        //choosenInteraction = GrabMethods.FAR_CONTROLLER;
        scriptAllRays.enabled = true;
        AllRaycastMethods.caseRay = (int)Method.FAR_RAYCAST;
        AllRaycastMethods.ActivateRay();
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.FAR_RAYCAST);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.FAR_RAYCAST);
        }
        else
        {
            tTest.setMethodID((int)Method.FAR_RAYCAST);
        }
        toggleMenue();
    }

    // in Indirect Method umbauen
    public void activateFarIndirect()
    {
      
        disableScripts();
   
        // IndirectRaycast.ActivateRay();
        //choosenInteraction = Method.FAR_INDIRECT_RAY;
        scriptAllRays.enabled = true;
        AllRaycastMethods.caseRay = (int)Method.FAR_INDIRECT_RAY;
        AllRaycastMethods.ActivateRay();
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.FAR_INDIRECT_RAY);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.FAR_INDIRECT_RAY);
        }
        else
        {
            tTest.setMethodID((int)Method.FAR_INDIRECT_RAY);
        }
        toggleMenue();

    }

    // entfernen
    public void activateFarGOGO()
    {
        //choosenInteraction = GrabMethods.FAR_GOGO;
    }

    public void enableTeaching()
    {
        teaching = !teaching;

        if (teaching)
        {
            menu1.buttons[2].ButtonIcon = ONTeachSprite;
            selfTeaching.toggleTeaching(true);
        }
        else
        {
            menu1.buttons[2].ButtonIcon = OFFTeachSprite;
            selfTeaching.toggleTeaching(false);
        }
        menu1.RegenerateButtons();



    }

    public void reset()
    {
        //Reload scene? 
        tTest.getMeasurements().isSucessful(0);

    }

    public void enableSnapping()
    {
        snap = !snap;

        if (snap)
        {
            menu1.buttons[1].ButtonIcon = ONSnappingSprite;
            tTest.getMeasurements().useSnapping(1);
        }
        else
        {
            menu1.buttons[1].ButtonIcon = OFFSnappingSprite;
            tTest.getMeasurements().useSnapping(0);
        }
        menu1.RegenerateButtons();
    }

    public void startMesure()
    {
        start = false;
        menu0.gameObject.SetActive(false);
        menu1.gameObject.SetActive(true);
        if (teaching) selfTeaching.increaseCounter();
        if (isLearnRoom)
        {

            learningtTest.initMeasurements();
            Debug.Log("Start Measure");

        }
        else
        {
            tTest.initMeasurements();
            tTest.getMeasurements().startGrabTimeMeasure();
            Debug.Log("Start Grab measure");
        }
    }


    public void stopLearning()
    {
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().stopAllTimeMeasures();
            long[] data = learningtTest.getMeasurements().packMeasurements();
            WriteMeasureFile wmf = new WriteMeasureFile();
            wmf.addLearningData2CSVFile(data);
        }

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
