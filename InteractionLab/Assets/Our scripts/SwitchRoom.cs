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
    public enum room { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4, supermarket2_5 };
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
       // if( GameObject.Find("Controller (right)").transform.GetComponent<BoxCollider>())
       if(other.gameObject.name == "Controller (right)")
        {
            currentScene = SceneManager.GetActiveScene();
            currentRoom = (room)currentScene.buildIndex;
            Debug.Log("ID: " + currentRoom);

            if (currentRoom == room.learn)
            {
                // Reload or reset all Scripts
                SceneManager.LoadScene((int)room.supermarket1_1, LoadSceneMode.Single);
                Debug.Log("load supermarkt1_1");
            }
            if (currentRoom == room.supermarket1_1)
            {
                SceneManager.LoadScene((int)room.supermarket1_2, LoadSceneMode.Single);
                Debug.Log("load supermarkt1_2");
            }
            if (currentRoom == room.supermarket1_2)
            {
                SceneManager.LoadScene((int)room.supermarket1_3, LoadSceneMode.Single);
                Debug.Log("load supermarkt1_3");
            }
            if (currentRoom == room.supermarket1_3)
            {
                SceneManager.LoadScene((int)room.supermarket2_1, LoadSceneMode.Single);
                Debug.Log("load supermarkt2_1");
            }
            if (currentRoom == room.supermarket2_1)
            {
                SceneManager.LoadScene((int)room.supermarket2_2, LoadSceneMode.Single);
                Debug.Log("load supermarkt2_2");
            }
            if (currentRoom == room.supermarket2_2)
            {
                SceneManager.LoadScene((int)room.supermarket2_3, LoadSceneMode.Single);
                Debug.Log("load supermarkt2_3");
            }
            if (currentRoom == room.supermarket2_3)
            {
                SceneManager.LoadScene((int)room.supermarket2_4, LoadSceneMode.Single);
                Debug.Log("load supermarkt2_4");
            }
            if (currentRoom == room.supermarket2_4)
            {
                SceneManager.LoadScene((int)room.supermarket2_5, LoadSceneMode.Single);
                Debug.Log("load supermarkt2_5");
            }
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
