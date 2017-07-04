using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Measurements
{
    private Stopwatch grabSW, posSW;
    enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };
    private long testID=1, grabTime = 0, posTime=0, errorRate = -1, WrongSelected = -1,Sucessful=0, Snapping=0;

    public Measurements()
    {
    }
  
      public long[] packMeasurements() {
        long[] data = { testID , grabTime, posTime, errorRate, WrongSelected, Sucessful, Snapping };
        return data;
    }


    public void incrementWrongSelection()
    {
        WrongSelected++;
    }
    public void incrementErrorRate() {
        errorRate++;
    }
    public void setTestID(long id) {
        testID = id;
    }

    public void useSnapping (long isUsed)
    {
        Snapping = isUsed;
    }

    public void isSucessful(long isSucess) {
        Sucessful = isSucess;
    } 

    public void startGrabTimeMeasure()
    {
        grabSW.Start();
        UnityEngine.Debug.Log("Start Grab Stopwatch");
    }

    public void startPosTimeMeasure()
    {
        posSW.Start();
        UnityEngine.Debug.Log("Start Pos Stopwatch");
    }

    public long StopGrabTimeMeasure()
    {
        if (grabSW.IsRunning)
        {
            grabSW.Stop();
            grabTime = grabSW.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Grab Stopwatch: " + grabSW.ElapsedMilliseconds + "ms");
            posSW.Start();
            UnityEngine.Debug.Log("Start Pos Stopwatch");
            return grabSW.ElapsedMilliseconds;
        }
        return 0;
    }

    public long StopPosTimeMeasure()
    {
        if (posSW.IsRunning)
        {
            posSW.Stop();
           posTime = posSW.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Pos Stopwatch: " + posSW.ElapsedMilliseconds + "ms");
            return posSW.ElapsedMilliseconds;
        }
        return 0;
    }

    public void initMeasurements()
    {
        grabSW = new Stopwatch();
        posSW = new Stopwatch();

    }

    public bool pauseGrabTimeMeasure()
    {
        grabSW.Stop();
        return grabSW.IsRunning;
    }

    public bool pausePosTimeMeasure()
    {
        posSW.Stop();
        return posSW.IsRunning;
    }


    public bool continueGrabTimeMeasure()
    {
        grabSW.Start();
        return grabSW.IsRunning;
    }

    public bool continuePosTimeMeasure()
    {
        posSW.Start();
        return posSW.IsRunning;
    }

    public long getGrabRoundTimeMeasure()
    {
        return grabSW.ElapsedMilliseconds;
    }

    public long getPosRoundTimeMeasure()
    {
        return posSW.ElapsedMilliseconds;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
