using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTest : MonoBehaviour
{
    private TimeMeasure grabTm;
    private TimeMeasure posTm;
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
        grabTm = new TimeMeasure();
        posTm = new TimeMeasure();
    }

    // Update is called once per frame
    void Update()
    {

        

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
        posTime= posTm.StopTimeMeasure();
        Debug.Log("Zeit" + posTime);
        Debug.Log("Enter");
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
