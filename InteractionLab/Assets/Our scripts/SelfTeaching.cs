using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;

public class SelfTeaching : MonoBehaviour
{
    private VRTK_ControllerTooltips controller_tips;

    // VRTK_ControllerEvents controllerTracked;

    private VRTK_ControllerTooltips.TooltipButtons button;
    private int counter;
    private string text;
    private bool shown;
    private bool startedTask;

    private GameObject targetObjTipp;
    private GameObject targetAreaTipp;
    private VRTK_ObjectTooltip infoBoard;
    private GameObject infoBoardCanvas;
    private VRTK_ObjectTooltip touch_tips;
    private VRTK_ObjectTooltip menu_tips;
    private VRTK_ObjectTooltip trigger_tips;
    private GameObject touch_canvas;
    private GameObject menu_canvas;
    private GameObject trigger_canvas;

    private int currentScene;
    private enum room { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };

    private List<int> buttonIDs;
    private List<int> isTexture;
    private List<int> buttonHeights;
    private List<string> buttonTexts;
    private int currentButton;

    public Material touch_grab;
    public Material proximity_grab;
    public Material wand_grab;
    public Material raycast;
    public Material extendable_ray;
    private Material currentMaterial;

    private WriteMeasureFile file;

    // Use this for initialization
    void Start()
    {
        controller_tips = GameObject.Find("ControllerTooltips").GetComponent<VRTK_ControllerTooltips>();
        touch_tips = GameObject.Find("ControllerTooltips/TouchpadTooltip").GetComponent<VRTK_ObjectTooltip>();
        menu_tips = GameObject.Find("ControllerTooltips/ButtonTwoTooltip").GetComponent<VRTK_ObjectTooltip>();
        trigger_tips = GameObject.Find("ControllerTooltips/TriggerTooltip").GetComponent<VRTK_ObjectTooltip>();
        touch_canvas = GameObject.Find("ControllerTooltips/TouchpadTooltip/TooltipCanvas/UIContainer");
        menu_canvas = GameObject.Find("ControllerTooltips/ButtonTwoTooltip/TooltipCanvas/UIContainer");
        trigger_canvas = GameObject.Find("ControllerTooltips/TriggerTooltip/TooltipCanvas/UIContainer");


        file = new WriteMeasureFile();
        file.initSelfteaching();
        buttonIDs = file.getButtonIds();
        isTexture = file.getIsTextures();
        buttonHeights = file.getHeights();
        buttonTexts = file.getTips();

        /*if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.Log("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
        }*/

        currentButton = 1;
        counter = 0;
        button = VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip;

        text = "";
        shown = true;
        startedTask = false;

        currentMaterial = null;
        currentScene = SceneManager.GetActiveScene().buildIndex;


        targetObjTipp = GameObject.Find("TargetObject/ObjectTooltip");
        targetObjTipp.SetActive(false);
        targetAreaTipp = GameObject.Find("TargetArea/ObjectTooltip");
        targetAreaTipp.SetActive(false);
 
    }

    // Update is called once per frame
    void Update()
    {
        if (AllRaycastMethods.StartIsReady)
        {
            controller_tips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
        }

        getSelfTeaching();
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
            shown = true;
        }

        if (!teaching)
        {
            controller_tips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
            //button = VRTK_ControllerTooltips.TooltipButtons.None;
            shown = false;
            targetAreaTipp.SetActive(false);
            targetObjTipp.SetActive(false);
 
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

    public void showTarget(bool target)
    {
        targetObjTipp.SetActive(target);
    }

    public void showArea(bool area)
    {
        targetAreaTipp.SetActive(area);
    }

    private void getSelfTeaching()
    {
        if (counter > 40) counter = 40;

        if (isTexture[counter] == -1)
        {
            text = buttonTexts[counter];
            getButton(buttonIDs[counter]);
            setMaterial();
            setHeight(buttonHeights[counter]);
            controller_tips.UpdateText(button, text);
            controller_tips.ToggleTips(shown, button);
        }
        else
        {
            text = ". ";
            controller_tips.UpdateText(button, text);
            getButton(buttonIDs[counter]);
            setMaterial();
            setHeight(buttonHeights[counter]);
            controller_tips.ToggleTips(true, button);
        }
    }

    private void getButton(int buttonID)
    {
        if (buttonID != currentButton)
        {
            controller_tips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);

            switch (buttonID)
            {
                case 0:
                    button = VRTK_ControllerTooltips.TooltipButtons.None;
                    break;
                case 1:
                    button = VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip;
                    break;
                case 2:
                    button = VRTK_ControllerTooltips.TooltipButtons.GripTooltip;
                    break;
                case 3:
                    button = VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip;
                    break;
                case 4:
                    button = VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip;
                    break;
                case 5:
                    button = VRTK_ControllerTooltips.TooltipButtons.ButtonTwoTooltip;
                    break;
                default:
                    break;
            }

            currentButton = buttonID;
            //controller_tips.ToggleTips(true, button);
        }
    }

    private void setMaterial()
    {
        switch (isTexture[counter])
        {
            case -1:
                currentMaterial = null;
                break;
            case (int)Menu.Method.CLOSE_SIMPLE:
                currentMaterial = touch_grab;
                break;
            case (int)Menu.Method.CLOSE_DIST:
                currentMaterial = proximity_grab;
                break;
            case (int)Menu.Method.CLOSE_ROD:
                currentMaterial = wand_grab;
                break;
            case (int)Menu.Method.FAR_RAYCAST:
                currentMaterial = raycast;
                break;
            case (int)Menu.Method.FAR_INDIRECT_RAY:
                currentMaterial = extendable_ray;
                break;
            default:
                break;
        }

        switch (currentButton)
        {
            case 0:
                break;
            case 1:
                trigger_canvas.GetComponent<Image>().material = currentMaterial;
                break;
            case 2:
               //gripButton
                break;
            case 3:
                touch_canvas.GetComponent<Image>().material = currentMaterial;
                break;
            case 4:
                //buttonOne
                break;
            case 5:
                menu_canvas.GetComponent<Image>().material = currentMaterial;
                break;
            default:
                break;
        }
    }

    private void setHeight(int buttonHeight)
    {
        Vector2 size = new Vector2();

        if (isTexture[counter] != -1)
        {
            size.x = 200;
            size.y = 30;
            //size.x = currentMaterial.mainTexture.width;
            //size.y = currentMaterial.mainTexture.height;
        }
        else
        {
            size.x = 200;
            size.y = buttonHeight;
        }

        switch (currentButton)
        {


            case 0:
                break;
            case 1:
                trigger_tips.containerSize = size;
                break;
            case 2:
                //gripButton
                break;
            case 3:
                touch_tips.containerSize = size;
                break;
            case 4:
                //ButtonOne
                break;
            case 5:
                menu_tips.containerSize = size;
                break;
            default:
                break;
        }


    }


}
