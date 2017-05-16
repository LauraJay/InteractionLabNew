using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public ControllerInfomations conInf = new ControllerInfomations();

    private List<t_Menue> menues;
    public int currentMenueHandle = -1;
    private int currentMenueId = 1;
    public bool menu = false;
    private GameObject emptyGameObj;

    // Use this for initialization
    void Start () {
        emptyGameObj = new GameObject();
        createMenu();
       
    }
	
	// Update is called once per frame
	void Update () {
        if(conInf.viveControllerConnected >= 1)
        {
            calculateMenu();
        }
       
    }

    //CREATE MENU
    private void createMenu()
    {
        //FILE MENU - START
        menues = new List<t_Menue>(new t_Menue[10]);
        menues[t_Menue.startMenu] = new t_Menue("start menu:", emptyGameObj);

        for (int i = 0; i < menues.Count; i++)
        {
            string tempFileName = i.ToString();

            menues[t_Menue.startMenu].addButton("open " + tempFileName, delegate { Debug.Log("Button gedrückt: " + i); });
        }
    }
            /*
            {
                switchToMenue(t_Menue.loadingScreen);

                
                menues[t_Menue.parameterMenu].addButton("changeColor", delegate { switchToMenue(t_Menue.colorPickerMenu); });
                menues[t_Menue.parameterMenu].addButton("back to main menu", delegate { switchToMenue(t_Menue.mainMenu); });

            });
        }
        menues[t_Menue.startMenu].addButton("back to main menu", delegate { switchToMenue(t_Menue.mainMenu); });

        //MAIN MENU
        menues[t_Menue.mainMenu] = new t_Menue("main menu:", uiObjects);
        menues[t_Menue.mainMenu].addButton("open file", delegate { switchToMenue(t_Menue.startMenu); });
        menues[t_Menue.mainMenu].addButton("change parameters", delegate { switchToMenue(t_Menue.parameterMenu); });
        menues[t_Menue.mainMenu].addButton("change mesh", delegate { switchToMenue(t_Menue.meshMenu); activeMesh(); });
        menues[t_Menue.mainMenu].addButton("show base", delegate { switchToMenue(t_Menue.baseMenu); });
        menues[t_Menue.mainMenu].addButton("show family", delegate { switchToMenue(t_Menue.familyMenu); });
        menues[t_Menue.mainMenu].addButton("show zone", delegate { switchToMenue(t_Menue.zoneMenu); });
        menues[t_Menue.mainMenu].addButton("config", delegate { switchToMenue(t_Menue.streamlineMenu); });
*/

    //SHOWING MENU OR NOT
    private void calculateMenu()
    {
        if (menu == true)
        {
            //menues[currentMenueId].gameObject.transform.parent = conInf.devices[currentMenueHandle].transform;
            menues[currentMenueId].gameObject.transform.position = conInf.controllerPosition[currentMenueHandle];
            menues[currentMenueId].gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            menues[currentMenueId].gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.2f);
            menues[currentMenueId].gameObject.transform.localRotation = Quaternion.AngleAxis(75, transform.right);
            menues[currentMenueId].gameObject.SetActive(true);
            
        }

        else
        {
            //menues[currentMenueId].gameObject.transform.SetParent(null);
            menues[currentMenueId].gameObject.SetActive(false);
        }


        if (conInf.menuPressedDown[0] || conInf.menuPressedDown[1])
        {
            if ((conInf.menuPressedDown[0] && (currentMenueHandle == 0)) || (conInf.menuPressedDown[1] && (currentMenueHandle == 1)))
            {
                menu = false;
                currentMenueHandle = -1;
            }

            else
            {
                menu = true;
                if (conInf.menuPressedDown[0])
                {
                    currentMenueHandle = 0;
                }
                else if (conInf.menuPressedDown[1])
                {
                    currentMenueHandle = 1;
                }
            }
        }
    }
}
