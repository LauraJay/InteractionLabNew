using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class TimeMeasure : MonoBehaviour {
    private Stopwatch sw;
	// Use this for initialization
	void Start () {
        sw = new Stopwatch();
        
	}

    public void startTimeMeasure()
    {
        sw = new Stopwatch();
        sw.Start();
    }

    public void clearTimeMeasure()
    {
        sw.Reset();
    }

    public long StopTimeMeasure()
    {
        sw.Stop();
        return sw.ElapsedMilliseconds;
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
