using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SelfTeaching : MonoBehaviour
{
    private VRTK_ControllerTooltips controller_tips_R;

    private VRTK_ControllerEvents controllerTracked;

    private VRTK_ControllerTooltips.TooltipButtons buttonR;
    private int counter;
    private string text;
    private bool shown;
    private bool startedTask;

    private GameObject targetObjTipp;
    private GameObject targetAreaTipp;
    private VRTK_ObjectTooltip infoBoard;
    private GameObject infoBoardCanvas;
    private VRTK_ObjectTooltip touch_tips_R;

    public Material testMaterial;

    // Use this for initialization
    void Start()
    {
        controller_tips_R = GameObject.Find("ControllerTooltipsR").GetComponent<VRTK_ControllerTooltips>();
        touch_tips_R = GameObject.Find("ControllerTooltipsR/TouchpadTooltip").GetComponent<VRTK_ObjectTooltip>();

        /*if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.Log("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
        }*/


        counter = 0;
        buttonR = VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip;

        text = "";
        shown = true;
        startedTask = false;

        targetObjTipp = GameObject.Find("TargetObject/ObjectTooltip");
        targetObjTipp.SetActive(false);
        targetAreaTipp = GameObject.Find("TargetArea/ObjectTooltip");
        targetAreaTipp.SetActive(false);
        infoBoard = GameObject.Find("board/ObjectTooltip").GetComponent<VRTK_ObjectTooltip>();
        infoBoardCanvas = GameObject.Find("board/ObjectTooltip/TooltipCanvas/UIContainer");
    }

    // Update is called once per frame
    void Update()
    {
        if (AllRaycastMethods.StartIsReady)
        {
            controller_tips_R.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
        }

        switch (counter)
        {
            case -1:
                break;
            case 0:
                controller_tips_R.ToggleTips(false, buttonR);
                buttonR = VRTK_ControllerTooltips.TooltipButtons.StartMenuTooltip;
                shown = true;
                counter++;
                //if (startedTask) counter += 2;
                break;
            case 1:
                text = "Menü aufrufen";
                break;
            case 2:
                controller_tips_R.ToggleTips(false, buttonR);
                buttonR = VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip;
                counter++;
                //if (startedTask) counter += 2;
                break;
            case 3:
                text = "Aufgabe starten";
                startedTask = true;
                break;
            case 4:
                text = "Grab auswählen";
                break;
                //NUMMERIERING
            case 5:
                //textR = "Methode auswählen";
                touch_tips_R.containerSize = new Vector2(150, 100);
                text = "Methode 1: blub \n Methode 2: blub \n Methode 1: blub \n Methode 2: blub \n Methode 3: blub";
                infoBoard.UpdateText("Methode 1: blub \n Methode 2: blub \n Methode 3: blub");
                infoBoardCanvas.GetComponent<Image>().material = testMaterial;
                break;
            case 6:
                controller_tips_R.ToggleTips(false, buttonR);
                buttonR = VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip;
                targetObjTipp.SetActive(true);
                counter++;
                break;
            case 7:
                text = "Target Objekt" + "\n" + " greifen";
                break;
            case 8:
                targetAreaTipp.SetActive(true);
                text = "auf Target Area legen";
                break;
            default:
                break;

        }

        controller_tips_R.ToggleTips(shown, buttonR);
        controller_tips_R.UpdateText(buttonR, text);

    }
    

    //Listener for Buttons?
    private void Pressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("touch Pressed");
        counter++;
    }

    public void toggleTeaching(bool teaching)
    {
        if (teaching)
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(Pressed);
            counter = 0;
        }
        
        if(!teaching)
        {
            controller_tips_R.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
            buttonR = VRTK_ControllerTooltips.TooltipButtons.None;
            shown = false;
            targetAreaTipp.SetActive(false);
            targetObjTipp.SetActive(false);
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed -= new ControllerInteractionEventHandler(Pressed);
            counter = -1;
        }
        
    }

    public void increaseCounter()
    {
        counter++;
    }

    public void setCounter(int number)
    {
        counter = number;
    }
}
