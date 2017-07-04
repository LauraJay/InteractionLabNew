using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SelfTeaching : MonoBehaviour
{

    private VRTK_ControllerTooltips controller_tips_L;
    private VRTK_ControllerTooltips controller_tips_R;

    private VRTK_ControllerEvents controllerTracked;

    private VRTK_ControllerTooltips.TooltipButtons buttonL;
    private VRTK_ControllerTooltips.TooltipButtons buttonR;
    private int counter;
    private string textL;
    private string textR;
    private bool left;
    private bool right;

    // Use this for initialization
    void Start()
    {
        controller_tips_L = GameObject.Find("ControllerTooltipsL").GetComponent<VRTK_ControllerTooltips>();
        controller_tips_R = GameObject.Find("ControllerTooltipsR").GetComponent<VRTK_ControllerTooltips>();

        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.Log("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
        }

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        counter = 0;
        buttonL = VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip;
        buttonR = VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip;

        textL = "";
        textR = "";
        left = true;
        right = true;


    }

    // Update is called once per frame
    void Update()
    {/*
        textR = "Wilkommen";
        left = false;
        right = true;

        controller_tips_L.ToggleTips(left, button);
        controller_tips_R.ToggleTips(right, button);

        controller_tips_R.UpdateText(button, textR);
        */
        if (RaycastingMethode.StartIsReady)
        {
            controller_tips_R.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
            controller_tips_L.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
        }

        switch (counter)
        {
            case 1:
                textL = "nächster Tipp";
                textR = "Wilkommen";
                break;
            case 2:
                controller_tips_R.ToggleTips(false, buttonR);
                buttonR = VRTK_ControllerTooltips.TooltipButtons.GripTooltip;
                textL = "weiter";
                textR = "macht nichts";
                break;
            case 3:
                textL = "immer weiter";
                left = true;
                right = false;
                break;
            case 4:
                buttonR = VRTK_ControllerTooltips.TooltipButtons.StartMenuTooltip;
                textL = "weiter";
                textR = "Menü aufrufen";
                left = true;
                right = true;
                break;
            default:
                break;

        }



        controller_tips_L.ToggleTips(left, buttonL);
        controller_tips_R.ToggleTips(right, buttonR);
        controller_tips_L.UpdateText(buttonL, textL);
        controller_tips_R.UpdateText(buttonR, textR);
        
        /*
               
         * 
        if (left && right)
        {
            controller_tips_L.UpdateText(buttonL, textL);
            controller_tips_R.UpdateText(buttonR, textR);
        }
        if (left && !right) controller_tips_L.UpdateText(button, textL);
        if (!left && right) controller_tips_R.UpdateText(button, textR);
        else Debug.Log("keine Anzeige");*/



    }
    
    private void deleteTipps()
    {
        controller_tips_R.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
        controller_tips_L.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
    }

    //Listener for Buttons?
    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("Trigger Pressed");
        counter++;
    }


}
