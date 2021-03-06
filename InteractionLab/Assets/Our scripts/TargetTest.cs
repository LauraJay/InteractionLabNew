﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    private Measurements m;
    private GameObject targetArea;
    private GameObject collidingObject;
    private enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };

    public int task = 1;
    public int method = 3;
    public int testId = 3;
    private int tempId = 5; // Id für jede Testperon anpassen

    public Material material;

    // Use this for initialization
    void Start()
    {
        targetArea = GameObject.Find("TargetArea");
        // add to menue start method


    }

    // Update is called once per frame
    void Update()
    {



    }

    public Measurements getMeasurements()
    {
        return m;
    }

    public void setTaskID(int taskId)
    {
        task = taskId;

    }
    public void setMethodID(int id)
    {
        method = id;

    }
    public void setTestID(int id)
    {
        testId = id;
        // m.setTestID(testId);
        m.setTestID(tempId);

    }

    public void initMeasurements()
    {
        m = new Measurements();
        m.initMeasurements();
        //m.setTestID(testId);
        m.setTestID(tempId);
    }


    public void OnCollisionEnter(Collision other)
    {
        collidingObject = other.gameObject;

        if (collidingObject.name.Equals("TargetArea"))
        {
            if (m != null)
            {
                m.StopPosTimeMeasure();
                m.StopTimeMeasure(method);
                m.setSucessfulMethod(method);
                m.isSucessful(1);
                Debug.Log("stop PosTIme ");
                targetArea.GetComponent<MeshRenderer>().material = material;
               
            }

        }
    }

    // public void OnTriggerStay(Collider other)
    // {
    //    Debug.Log("Stay");
    //     collidingObject.GetComponent<MeshRenderer>().materials.SetValue(testMaterial, 0);
    // }


    public void OnCollisionExit(Collision other)
    {
        Debug.Log("Exit");
    }

}
