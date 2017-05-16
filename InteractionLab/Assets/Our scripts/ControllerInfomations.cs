using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerInfomations : MonoBehaviour {

    private List<int> controllerIndices = new List<int>();
    public int viveControllerConnected = 0;
    private List<Collider> currentTriggeredObject = new List<Collider>();

    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId touchPad0 = Valve.VR.EVRButtonId.k_EButton_Axis0;
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;

    public bool[] triggerPressedDown = { false, false };
    public bool[] triggerPressedUp = { false, false };
    public bool[] triggerPressed = { false, false };
    public bool[] touchpadTouched = { false, false };
    public Vector2[] touchpadPosition = { new Vector2(), new Vector2() };
    public bool[] menuPressed = { false, false };
    public bool[] menuPressedDown = { false, false };
    public bool[] gripPressed = { false, false };
    public bool[] gripPressedDown = { false, false };
    public bool[] gripPressedUp = { false, false };
    public Vector3[] controllerPosition = { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
    public Quaternion[] controllerRotation = { new Quaternion(), new Quaternion() };

    public SteamVR_Controller.Device device1;
    public SteamVR_Controller.Device device2;

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
		
	}
	
	// Update is called once per frame
	void Update () {
        if (viveControllerConnected >= 1)
        {
            //GET CONTROLLER DATA
            SteamVR_Controller.Device device;

            for (int deviceId = 0; deviceId < controllerIndices.Count; deviceId++)
            {
                device = SteamVR_Controller.Input(controllerIndices[deviceId]);
                
                if (device != null)
                {
                    triggerPressedDown[deviceId] = device.GetPressDown(trigger);
                    triggerPressedUp[deviceId] = device.GetPressUp(trigger);
                    triggerPressed[deviceId] = device.GetPress(trigger);
                    menuPressed[deviceId] = device.GetPress(menuButton);
                    menuPressedDown[deviceId] = device.GetPressDown(menuButton);
                    gripPressed[deviceId] = device.GetPress(grip);
                    gripPressedDown[deviceId] = device.GetPressDown(grip);
                    gripPressedUp[deviceId] = device.GetPressUp(grip);
                    touchpadTouched[deviceId] = device.GetTouch(touchPad);
                    touchpadPosition[deviceId] = device.GetAxis(touchPad);
                    controllerPosition[deviceId] = device.transform.pos;
                    controllerRotation[deviceId] = device.transform.rot;
                }

                if (deviceId == 0)
                    device1 = device;

                if (deviceId == 1)
                    device2 = device;
            }
        }
    }
}
