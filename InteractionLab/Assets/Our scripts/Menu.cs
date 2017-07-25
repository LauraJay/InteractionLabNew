using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class Menu : MonoBehaviour
{

    //scripts for activating grabbing methods
    private TouchGrab scriptNearSimple;
    private ProximityGrab scriptNearDist;
    private AllRaycastMethods scriptAllRays;

    public static bool teaching = false;
    public bool snap;
    public bool showingTask;
    public bool menuVisible;
    public bool activateSwitchRoom;
    public bool isLearnRoom = false;
    private int currentRoom;

    public static bool RodIsEnabled = false;

    private SelfTeaching selfTeaching;
    private showTasks showTasks;

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
    private int counter;

    private GameObject switchScene;

    private TargetTest tTest;
    private LearningTargetTest learningtTest;
    public enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };
    public Method choosenInteraction;


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
        snap = false;
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
            teaching = true;
            learningtTest = GameObject.Find("TargetObject").GetComponent<LearningTargetTest>();
            learningtTest.initMeasurements();
            selfTeaching = GameObject.Find("RightController").GetComponent<SelfTeaching>();
        }
        else
        {
            teaching = false;
            tTest = GameObject.Find("TargetObject").GetComponent<TargetTest>();
            tTest.initMeasurements();
            showTasks = GameObject.Find("RightController").GetComponent<showTasks>();
            scriptAllRays = GameObject.Find("Controller (right)").GetComponent<AllRaycastMethods>();
            scriptAllRays.enabled = true;
            scriptAllRays.setCounter(0);
        }

        counter = 0;
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

            //currentRoom = SceneManager.GetActiveScene().buildIndex;
            //switch (currentRoom)
            //{
            //    case 4:
            //        activateNearSimple();
            //        break;
            //    case 5:
            //        activateFarIndirect();
            //        break;
            //    case 6:
            //        activateNearRod();
            //        break;
            //    case 7:
            //        activateNearDist();
            //        break;
            //    case 8:
            //        activateFarController();
            //        break;
            //}
        }
    }

    //Get all interaction scripts
    void GetScripts()
    {
        scriptNearSimple = GameObject.Find("Controller (right)").GetComponent<TouchGrab>();
        scriptNearDist   = GameObject.Find("Controller (right)").GetComponent<ProximityGrab>();
        scriptAllRays    = GameObject.Find("Controller (right)").GetComponent<AllRaycastMethods>();
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
            if (teaching) selfTeaching.increaseCounter();
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
        disableScripts();
        choosenInteraction = Method.CLOSE_SIMPLE;
        scriptNearSimple.enabled = true;

        if (isLearnRoom)
        {
            selfTeaching.showTarget(true);
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_SIMPLE);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_SIMPLE);
        }
        else
        {
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            tTest.setMethodID((int)Method.CLOSE_SIMPLE);
            tTest.getMeasurements().startTimeMeasure(tTest.method);
        }
        if (teaching) selfTeaching.setCounter(3); //COUNTER-1 gesetzt wegen toggel
        toggleMenue();
    }

    public void activateNearRod()
    {
        disableScripts();

        scriptAllRays.enabled = true;
        AllRaycastMethods.caseRay = (int)Method.CLOSE_ROD;
        AllRaycastMethods.ActivateRay();
        choosenInteraction = Method.CLOSE_ROD;
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_ROD);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_ROD);
        }
        else
        {
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            tTest.setMethodID((int)Method.CLOSE_ROD);
            tTest.getMeasurements().startTimeMeasure(tTest.method);
        }
        if (teaching) selfTeaching.setCounter(25); //COUNTER-1 gesetzt wegen toggel
        toggleMenue();
    }

    public void activateNearDist()
    {
        disableScripts();
        scriptNearDist.enabled = true;
        choosenInteraction = Method.CLOSE_DIST;
        if (isLearnRoom)
        {
            learningtTest.getMeasurements().StopTimeMeasure(learningtTest.getMethodID());
            learningtTest.setMethodID((int)Method.CLOSE_DIST);
            learningtTest.getMeasurements().startTimeMeasure((int)Method.CLOSE_DIST);
        }
        else
        {
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            tTest.setMethodID((int)Method.CLOSE_DIST);
            tTest.getMeasurements().startTimeMeasure(tTest.method);


        }
        if (teaching) selfTeaching.setCounter(14); //COUNTER-1 gesetzt wegen toggel
        toggleMenue();
    }

    public void activateFarController()
    {
        //if (teaching) selfTeaching.increaseCounter();
        disableScripts();
        choosenInteraction = Method.FAR_RAYCAST;
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
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            tTest.setMethodID((int)Method.FAR_RAYCAST);
            tTest.getMeasurements().startTimeMeasure(tTest.method);


        }
        if (teaching) selfTeaching.setCounter(30); //COUNTER-1 gesetzt wegen toggel
        toggleMenue();
    }


    public void activateFarIndirect()
    {
        disableScripts();
        choosenInteraction = Method.FAR_INDIRECT_RAY;
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
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            tTest.setMethodID((int)Method.FAR_INDIRECT_RAY);
            tTest.getMeasurements().startTimeMeasure(tTest.method);

        }
        if (teaching) selfTeaching.setCounter(35); //COUNTER-1 gesetzt wegen toggel
        toggleMenue();
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
        //Reload scene
        currentRoom = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentRoom, LoadSceneMode.Single);

        tTest.getMeasurements().isSucessful(0);

    }

    public void enableSnapping()
    {
        snap = !snap;

        if (snap)
        {
            menu1.buttons[1].ButtonIcon = ONSnappingSprite;
            if (!isLearnRoom) tTest.getMeasurements().useSnapping(1);
        }
        else
        {
            menu1.buttons[1].ButtonIcon = OFFSnappingSprite;
            if (!isLearnRoom) tTest.getMeasurements().useSnapping(0);
        }
        menu1.RegenerateButtons();

        if (teaching) selfTeaching.increaseCounter();

        if (choosenInteraction == Method.CLOSE_SIMPLE) scriptNearSimple.setObjectSnapping(snap);
        if (choosenInteraction == Method.CLOSE_DIST) scriptNearDist.setObjectSnapping(snap);
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

        currentRoom = SceneManager.GetActiveScene().buildIndex;
        switch (currentRoom)
        {
            case 4:
                activateNearSimple();
                break;
            case 5:
                activateNearRod();
                break;
            case 6:
                activateFarIndirect();
                break;
            case 7:
                activateNearDist();
                break;
            case 8:
                activateFarController();
                break;
        }
    }


    public void stopMeasure()
    {
        if (isLearnRoom)
        {
            selfTeaching.toggleTeaching(true);
            learningtTest.getMeasurements().stopAllTimeMeasures();
            long[] data = learningtTest.getMeasurements().packMeasurements();
            WriteMeasureFile wmf = new WriteMeasureFile();
            wmf.addLearningData2CSVFile(data);
        }
        else
        {
            tTest.getMeasurements().StopTimeMeasure(tTest.method);
            WriteMeasureFile wmf = new WriteMeasureFile();
            if (tTest.task > 3)
            {
                long[] data = tTest.getMeasurements().packConstraintedMethodMeasurements();
                wmf.addContrainedMethodData2CSVFile(tTest.task, tTest.method, data);
            }
            else if (tTest.task > 0 && tTest.task <= 3) {
                long[] data = tTest.getMeasurements().packSelfChoosedMethodMeasurements();
                wmf.addChoosenMethodData2CSVFile(data, tTest.task);

            }
        }

        if (teaching) selfTeaching.setCounter(39);
        toggleMenue();

        disableScripts();
        teaching = false;
        if (currentRoom != 8)
        {
            switchScene.SetActive(true);
            switchScene.GetComponent<BoxCollider>().isTrigger = true;
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
        showTasks.toggleTask(showingTask);
    }

    public void switchRoom()
    {
        activateSwitchRoom = true;
        switchScene.SetActive(true);
    }


    private List<Method> intialiseMethodList()
    {
        List<Method> init = new List<Method>();
        for (int i = 0; i < (int)Method.FAR_INDIRECT_RAY; i++)
        {
            init.Add((Method)i);
        }
        return init;
    }

    private List<Method> intialiseCloseList()
    {
        List<Method> init = new List<Method>();
        for (int i = (int)Method.CLOSE_SIMPLE; i < (int)Method.CLOSE_ROD; i++)
        {
            init.Add((Method)i);
        }
        return init;
    }
    private List<Method> intialiseFarList()
    {
        List<Method> init = new List<Method>();
        for (int i = (int)Method.FAR_RAYCAST; i < (int)Method.FAR_INDIRECT_RAY; i++)
        {
            init.Add((Method)i);
        }
        return init;
    }

    private List<Method> radomizeMethodCall(List<Method> list)
    {
        System.Random r = new System.Random();
        List<Method> randomized = new List<Method>();
        while (list.Count > 0)
        {
            int n = r.Next(list.Count);
            randomized.Add(list[n]);
            list.RemoveAt(n);
        }
        return randomized;
    }
}
