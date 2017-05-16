using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using UnityEngine;
using System;
using System.IO;

public class t_Menue
{
    //MENU IDS
    public static readonly int startMenu = 1;


    //OBJECTS
    private Canvas canvas;
    private List<Button> buttons;
    private GameObject cube;
    private Cubemap picker;


    //ADJUSTMENTS
    public string name;
    private float buttonWidth = 0.8f;
    private float buttonHeight = 0.12f;
    private float buttonDepth = 0.2f;
    private float buttonDistance = 0.05f;
    private float canvasWidth = 1f;
    private float canvasHeight = 2f;
    private float border = 0.08f;

    private int activeButton = -1;


    public GameObject gameObject;

    public t_Menue(string name, GameObject parent)
    {
        this.name = name;
        this.buttons = new List<Button>();

        //GAME OBJECT
        gameObject = new GameObject();
        gameObject.transform.parent = parent.transform;
        gameObject.name = "Menue_" + name;
        gameObject.SetActive(false);

        //CANVAS
        GameObject canvasObject = GameObject.Instantiate(Resources.Load("canvasObject")) as GameObject;
        canvas = canvasObject.GetComponent<Canvas>();
        canvas.name = "Canvas_" + name;
        canvas.transform.SetParent(gameObject.transform, false);

        RectTransform rt = canvas.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(canvasWidth, canvasHeight);

        //CUBE BEHIND BUTTONS
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(1f, 1f, 0.01f);
        //cube.transform.localPosition = new Vector3( 0f, 0.6f, 0.01f );
        cube.transform.SetParent(gameObject.transform, false);

    }

    //ADD BUTTON TO CANVAS
    public void addButton(string name, UnityAction<Hand> call)
    {
        GameObject buttonObject = GameObject.Instantiate(Resources.Load("buttonObject")) as GameObject;
        Button button = buttonObject.GetComponent<Button>();
        button.transform.SetParent(canvas.transform, false);
        
        button.name = "Button_" + name;
        button.GetComponentInChildren<Text>().text = name;
        float translateY = (buttonHeight + buttonDistance) * buttons.Count;
        translateY = (canvasHeight / 2) - translateY;
        button.transform.position = new Vector2(0f, translateY);

        GameObject buttonCollider = buttonObject.transform.Find("buttonCollider").gameObject;
        buttonCollider.transform.localScale = new Vector3(buttonWidth, buttonHeight, buttonDepth);

        RectTransform rt = button.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(buttonWidth, buttonHeight);

        buttons.Add(button);
        button.GetComponent<UIElement>().onHandClick.AddListener(call);

        setBackgroundPosition();
    }



    //CHANGE COLOR OF BUTTONS
    public void setBackgroundPosition()
    {
        float cubeHeight = buttons.Count * buttonHeight + (buttons.Count - 1) * buttonDistance + border; //size of the cube dependend on number of buttons
        float cubePos = border + (canvasHeight / 2) - (cubeHeight / 2f); //position of the cube dependend on the size
        cube.transform.localScale = new Vector3(1f, cubeHeight, 0.01f);
        cube.transform.localPosition = new Vector3(0f, cubePos, 0.01f);
    }

    //CHANGE COLOR OF BUTTONS
    public void changeColor(Color color, int buttonId)
    {
        ColorBlock colors = buttons[buttonId].GetComponent<Button>().colors;
        colors.normalColor = color;
        buttons[buttonId].GetComponent<Button>().colors = colors;
    }

    public void changeColor(Color color)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            changeColor(color, i);
        }
    }

    /*public void enableColorPicker()
    {
        //GameObject picker = new GameObject();
        // GameObject colorWheelObject = GameObject.Instantiate(Resources.Load("colorWheelObject")) as GameObject;
        GameObject colorWheelObject = GameObject.Instantiate(Resources.Load("colorPlaneObject")) as GameObject;
        colorWheelObject.transform.parent = gameObject.transform;
        colorWheelObject.SetActive(true);
        //colorWheelObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.01f);
        //colorWheelObject.transform.localPosition = new Vector3(0f, -0.8f, -0.29f);
        colorWheelObject.transform.localScale = new Vector3(0.015f, 0.01f, 0.015f);
        colorWheelObject.transform.localPosition = new Vector3(0f, -0.81f, -0.27f);
        colorWheelObject.transform.localEulerAngles = new Vector3(-220f, -82.5f, 100f);

        String path = Application.dataPath + "/Images/Color_circle_(hue-sat).png"; //load texture for color wheel
        byte[] tex = File.ReadAllBytes(path);
        Texture2D HSVWheel = new Texture2D(480, 480);
        HSVWheel.LoadImage(tex);
        colorWheelObject.GetComponent<Renderer>().material.mainTexture = HSVWheel;   //TODO - better: texture only on one face of the cube!

    }

    public void setActive(int buttonId)
    {
        changeColor(Color.blue);
        if (buttonId != -1)
        {
            changeColor(Color.red, buttonId);
        }
        activeButton = buttonId;
    }

    public void toggleActive(int buttonId)
    {
        if (buttonId != -1)
        {
            if (getActive() == buttonId)
            {
                activeButton = -1;
                changeColor(Color.blue, buttonId);
            }
            else
            {
                setActive(buttonId);
            }
        }
    }


    public int getActive()
    {
        return activeButton;
    }

    public void setBaseName(String buttonName)
    {
        baseButton.GetComponentInChildren<Text>().text = buttonName;
    }
    */
}









