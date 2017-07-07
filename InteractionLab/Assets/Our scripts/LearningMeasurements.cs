using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class LearningMeasurements
{
    private Stopwatch SW_C_Simple, SW_C_Dist, SW_C_ROD, SW_F_RAYC, SW_F_INDIR;
    enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };
    private long testID = 1, time_C_Simple = 0, time_C_Dist = 0, time_C_ROD = 0, time_F_RAYC = 0, time_F_INDIR = 0;

    public LearningMeasurements()
    {
    }

    public long[] packMeasurements()
    {
        long[] data = { testID, time_C_Simple, time_C_Dist, time_C_ROD, time_F_RAYC, time_F_INDIR };
        return data;
    }


    public void setTestID(long id)
    {
        testID = id;
    }

    public void stopAllTimeMeasures() {

        
            SW_C_Simple.Stop();
            time_C_Simple = SW_C_Simple.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Simple Stopwatch: " + SW_C_Simple.ElapsedMilliseconds + "ms");
       
            SW_C_Dist.Stop();
            time_C_Dist = SW_C_Dist.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Dist Stopwatch: " + SW_C_Dist.ElapsedMilliseconds + "ms");
       
            SW_C_ROD.Stop();
            time_C_ROD = SW_C_ROD.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Rod Stopwatch: " + SW_C_ROD.ElapsedMilliseconds + "ms");
        
            SW_F_RAYC.Stop();
            time_F_RAYC = SW_F_RAYC.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Raycast Stopwatch: " + SW_F_RAYC.ElapsedMilliseconds + "ms");
       
            SW_F_INDIR.Stop();
            time_F_INDIR = SW_F_INDIR.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Stop Indirect Stopwatch: " + SW_F_INDIR.ElapsedMilliseconds + "ms");
        
    }    
    public void startTimeMeasure(int method)
    {
        switch (method)
        {
            case (int)Method.CLOSE_SIMPLE:
                SW_C_Simple.Start();
                break;
            case (int)Method.CLOSE_DIST:
                SW_C_Dist.Start();
                break;
            case (int)Method.CLOSE_ROD:
                SW_C_ROD.Start();
                break;
            case (int)Method.FAR_RAYCAST:
               SW_F_RAYC.Start();
                break;
            case (int)Method.FAR_INDIRECT_RAY:
                SW_F_INDIR.Start();
                break;
            default:
                break;
        }
        UnityEngine.Debug.Log("Start Stopwatch" + method);
    }

   
    public void StopTimeMeasure(int method)
    {
        switch (method)
        {
            case (int)Method.CLOSE_SIMPLE:
                if (SW_C_Simple.IsRunning)
                {
                    SW_C_Simple.Stop();
                    time_C_Simple = SW_C_Simple.ElapsedMilliseconds;
                    UnityEngine.Debug.Log("Stop Simple Stopwatch: " + SW_C_Simple.ElapsedMilliseconds + "ms");
                }
                break;
            case (int)Method.CLOSE_DIST:
                if (SW_C_Dist.IsRunning)
                {
                    SW_C_Dist.Stop();
                    time_C_Dist = SW_C_Dist.ElapsedMilliseconds;
                    UnityEngine.Debug.Log("Stop Dist Stopwatch: " + SW_C_Dist.ElapsedMilliseconds + "ms");
                }
                break;
            case (int)Method.CLOSE_ROD:
                if (SW_C_ROD.IsRunning)
                {
                    SW_C_ROD.Stop();
                    time_C_ROD = SW_C_ROD.ElapsedMilliseconds;
                    UnityEngine.Debug.Log("Stop Rod Stopwatch: " + SW_C_ROD.ElapsedMilliseconds + "ms");
                }
                break;
            case (int)Method.FAR_RAYCAST:
                if (SW_F_RAYC.IsRunning)
                {
                    SW_F_RAYC.Stop();
                    time_F_RAYC = SW_F_RAYC.ElapsedMilliseconds;
                    UnityEngine.Debug.Log("Stop Raycast Stopwatch: " + SW_F_RAYC.ElapsedMilliseconds + "ms");
                }
                break;
            case (int)Method.FAR_INDIRECT_RAY:
                if (SW_F_INDIR.IsRunning)
                {
                    SW_F_INDIR.Stop();
                    time_F_INDIR = SW_F_INDIR.ElapsedMilliseconds;
                    UnityEngine.Debug.Log("Stop Indirect Stopwatch: " + SW_F_INDIR.ElapsedMilliseconds + "ms");
                }
                break;
            default:
                break;
        }
    }

   

    public void initMeasurements()
    {
        SW_C_Simple = new Stopwatch();
        SW_C_Dist = new Stopwatch();
        SW_C_ROD = new Stopwatch();
        SW_F_INDIR = new Stopwatch();
        SW_F_RAYC = new Stopwatch();

    }

    
    
    // Update is called once per frame
    void Update()
    {
    }
}
