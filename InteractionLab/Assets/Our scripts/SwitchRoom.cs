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
    private enum room { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    private room currentRoom;
   // private int learning = 0;
    //private int supermarketAufg1 = 1;


    // Use this for initialization
    void Start () {

       

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider other)
    {
        currentScene = SceneManager.GetActiveScene();
        currentRoom = (room) currentScene.buildIndex;
        Debug.Log("ID: " + currentRoom);
        
        if (currentRoom == room.learn)
        {
            SceneManager.LoadScene((int)room.supermarket1_1, LoadSceneMode.Single);
            Debug.Log("load supermarkt1_1");
        }
        if (currentRoom == room.supermarket1_1)
        {
            SceneManager.LoadScene((int)room.supermarket1_2, LoadSceneMode.Single);
            Debug.Log("load supermarkt1_2");
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
