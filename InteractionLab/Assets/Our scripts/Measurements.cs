using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Measurements : MonoBehaviour {
    private Stopwatch sw;
	// Use this for initialization
	void Start () {
        
	}

    public void startTimeMeasure()
    {
        sw.Start();
        //UnityEngine.Debug.Log("Start Stopwatch");
    }

    public void clearTimeMeasure()
    {
        sw.Reset();
    }

    public long StopTimeMeasure()
    {
        if (sw.IsRunning)
        {
        sw.Stop();
        UnityEngine.Debug.Log("Stop Stopwatch: "+ sw.ElapsedMilliseconds+"ms");
        return sw.ElapsedMilliseconds;
        }
        return 0;
    }
    public void initMeasurements() {
        sw = new Stopwatch();

    }

    public bool pauseTimeMeasure()
    {
        sw.Stop();
        return sw.IsRunning;
    }

    public bool continueTimeMeasure()
    {
        sw.Start();
        return sw.IsRunning;
    }

    public long getRoundTimeMeasure()
    {
       return sw.ElapsedMilliseconds;
    }

    // Update is called once per frame
    void Update () {
     	}
}
