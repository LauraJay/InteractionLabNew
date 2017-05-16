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

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

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
    void Start () {/*
        string dir = System.Environment.CurrentDirectory;

        myLoadedAssetBundle = AssetBundle.LoadFromFile(dir + "\\Assets\\Scenes");
        scenePaths = myLoadedAssetBundle.GetAllScenePaths();*/

    }

    // Update is called once per frame
    void Update()
    {
        if (viveControllerConnected >= 1)
        {
            
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
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("stay");
        Scene currentScene = SceneManager.GetActiveScene();
        int currentID = currentScene.buildIndex;
        int learning = 0;
        int supermarket = 1;

        Debug.Log("ID: " + currentID);
       //SceneManager.LoadScene(supermarket, LoadSceneMode.Single);
       /*if (gripPressedDown[0] || gripPressedDown[1])
        {
            Debug.Log("grip");

        }*/
        if ((gripPressedDown[0] || gripPressedDown[1]) && currentID==learning)
         {
            SceneManager.LoadScene(supermarket);
            Debug.Log("supermarkt");
        }
        if ((gripPressedDown[0] || gripPressedDown[1]) && currentID == supermarket)
        {
            SceneManager.LoadScene(learning);
        }
         // Application.LoadLevel(supermarket);

    }
}
