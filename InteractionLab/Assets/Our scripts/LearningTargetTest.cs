using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningTargetTest : MonoBehaviour
{
    private LearningMeasurements m;
    private GameObject targetArea;
    private GameObject collidingObject;
    private enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };

    public int task = 1;
    public int method = 3;
    public int testId = 3;

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

    public LearningMeasurements getMeasurements()
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

    public int getMethodID() {
        return method;
    }
    public void setTestID(int id)
    {
        testId = id;
        m.setTestID(testId);

    }

    public void initMeasurements()
    {
        m = new LearningMeasurements();
        m.initMeasurements();
        m.setTestID(testId);
    }


    public void OnCollisionEnter(Collision other)
    {
        collidingObject = other.gameObject;

        if (collidingObject.name.Equals("TargetArea"))
        {
            //if (m != null)
            //{
            //    m.StopTimeMeasure(method);
            //    Debug.Log("stop PosTIme ");
                targetArea.GetComponent<MeshRenderer>().material = material;
                //long[] data = m.packMeasurements();
                //WriteMeasureFile wmf = new WriteMeasureFile();
                //wmf.addData2CSVFile((int)task, (int)method, data);

            //}

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
