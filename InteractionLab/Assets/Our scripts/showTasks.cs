using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class showTasks : MonoBehaviour
{
    private VRTK_ControllerTooltips controller_tips;
    private VRTK_ObjectTooltip tasks_tips;
    private VRTK_ObjectTooltip infoBoard;
    private GameObject infoBoardCanvas;
    private int currentRoom;


    private string textTask;
    private string textBoard;
    bool shown;

    private WriteMeasureFile fileTask;
    private List<string> buttonTextsTask;
    private List<int> buttonHeightsTask;
    private WriteMeasureFile fileBoard;
    private List<string> buttonTextsBoard;
    private List<int> buttonHeightsBoard;


    // Use this for initialization
    void Start()
    {
        controller_tips = GameObject.Find("ControllerTooltips").GetComponent<VRTK_ControllerTooltips>();
        tasks_tips = GameObject.Find("ControllerTooltips/ButtonOneTooltip").GetComponent<VRTK_ObjectTooltip>();
        infoBoard = GameObject.Find("board/ObjectTooltip").GetComponent<VRTK_ObjectTooltip>();
        shown = true;

        fileTask = new WriteMeasureFile();
        fileTask.initTasks();
        buttonHeightsTask = fileTask.getHeights();
        buttonTextsTask = fileTask.getTips();

        fileBoard = new WriteMeasureFile();
        fileBoard.initBoard();
        buttonHeightsBoard = fileBoard.getHeights();
        buttonTextsBoard = fileBoard.getTips();
    }

    // Update is called once per frame
    void Update()
    {
        currentRoom = SceneManager.GetActiveScene().buildIndex;


        if (currentRoom <= 3)
        {
            textBoard = buttonTextsBoard[0];
            infoBoard.containerSize = setHeight(buttonHeightsBoard[0]);
            infoBoard.UpdateText(textBoard);

            if (shown)
            {
                textTask = buttonTextsTask[currentRoom];
                tasks_tips.containerSize = setHeight(buttonHeightsTask[currentRoom]);
                controller_tips.UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip, textTask);
                controller_tips.ToggleTips(shown, VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip);
            }
        }

        else
        {
            if (shown)
            {
                textTask = buttonTextsTask[4];
                tasks_tips.containerSize = setHeight(buttonHeightsTask[4]);
                controller_tips.UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip, textTask);
                controller_tips.ToggleTips(shown, VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip);
            }

            textBoard = buttonTextsBoard[currentRoom];
            infoBoard.containerSize = setHeight(buttonHeightsBoard[currentRoom]);
            infoBoard.UpdateText(textBoard);
        }
    }


    private Vector2 setHeight(int buttonHeight)
    {
        Vector2 size = new Vector2();
        size.x = 200;
        size.y = buttonHeight;

        return size;
    }

    public void toggleTask(bool task)
    {
        if (task)
        {
            shown = true;
        }

        if (!task)
        {
            controller_tips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.None);
            shown = false;
        }

    }
}
