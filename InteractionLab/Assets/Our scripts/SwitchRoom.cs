using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SwitchRoom : MonoBehaviour {


    private List<int> controllerIndices = new List<int>();
    private int viveControllerConnected = 0;
    private List<Collider> currentTriggeredObject = new List<Collider>();
    private bool[] gripPressedDown = { false, false };
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;

    private Scene currentScene;
    private int currentID;
    private bool alreadyChanged;

    //CONS - Buildsettings
    private int learning = 0;
    private int supermarket = 1;

    private void OnDeviceConnected(int deviceid, bool isConnected)
    {
        int index = deviceid;

        var system = OpenVR.System;
        if (system == null || system.GetTrackedDeviceClass((uint)index) != ETrackedDeviceClass.Controller)
            return;


        if (isConnected)
        {
            Debug.Log(string.Format("Controller {0} connected.", index));
            controllerIndices.Add(index);
            currentTriggeredObject.Add(null);
            currentTriggeredObject.Add(null);
            viveControllerConnected++;
        }
        else
        {
            Debug.Log(string.Format("Controller {0} disconnected.", index));
            controllerIndices.Remove(index);
        }
    }

    void OnEnable()
    {
        SteamVR_Events.DeviceConnected.Listen(OnDeviceConnected);
    }

    void OnDisable()
    {
        SteamVR_Events.DeviceConnected.Remove(OnDeviceConnected);
    }

    // Use this for initialization
    void Start () {
        alreadyChanged = false;
       

    }

    // Update is called once per frame
    void Update()
    {

        if (viveControllerConnected >= 1)
        {
            //Debug.Log("connected");
            //GET CONTROLLER DATA
            SteamVR_Controller.Device device;

            for (int deviceId = 0; deviceId < controllerIndices.Count; deviceId++)
            {
                device = SteamVR_Controller.Input(controllerIndices[deviceId]);
                
                if (device != null)
                { 
                    gripPressedDown[deviceId] = device.GetPressDown(grip);

                }
            }

        }

        //Debug.Log("grip pressed? " + gripPressedDown[0]);
    }

    void OnTriggerEnter(Collider other)
    {
        currentScene = SceneManager.GetActiveScene();
        currentID = currentScene.buildIndex;
        Debug.Log("ID: " + currentID);
        
        if (currentID == learning)
        {
            SceneManager.LoadScene(supermarket, LoadSceneMode.Single);
            Debug.Log("load supermarkt");
        }
        if (currentID == supermarket)
        {
            SceneManager.LoadScene(learning, LoadSceneMode.Single);
            Debug.Log("load learning");
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("stay");

    }

    void OnTriggerExit(Collider other)
    {
        //currentID = -1;
        Debug.Log("EXIT");
    }
}
