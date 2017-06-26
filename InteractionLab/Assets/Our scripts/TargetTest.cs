using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    private Measurements grabTm;
    private Measurements posTm;
    private GameObject targetArea;
    private GameObject collidingObject;
    private long grabTime = 0;
    private long posTime = 0;


    public Material material;

    // Use this for initialization
    void Start()
    {
        targetArea = GameObject.Find("TargetArea");
        collidingObject = GameObject.FindGameObjectWithTag("Moveable");
        grabTm = new Measurements();
        grabTm.initMeasurements();
        posTm = new Measurements();
        posTm.initMeasurements();
    }

    // Update is called once per frame
    void Update()
    {

        

    }

    public void startPosTime()
    {
        posTm.startTimeMeasure();
    }

    public void stopPosTIme()
    {
        posTime = posTm.StopTimeMeasure();
        
    }

    public void startGrabTime() {
        grabTm.startTimeMeasure();
    }

    public void stopGrabTIme() {
        grabTime = grabTm.StopTimeMeasure();
        posTm.startTimeMeasure();
    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enter");
        stopPosTIme();
        targetArea.GetComponent<MeshRenderer>().material = material;
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
