using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WriteTimeMeasureFile : MonoBehaviour {
    private string path;
    FileStream fs;
    enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    // Use this for initialization
    void Start () {
    }

    private bool initCSVFile(int room) {
        string fileName = "";
        switch (room)
        {
            case (int)ROOM.learn:
                fileName = "Learning.csv";
                break;
            case (int)ROOM.supermarket1_1:
                fileName = "Task_1_Subtask_1.csv";
                break;
            case (int)ROOM.supermarket1_2:
                fileName = "Task_1_Subtask_2.csv";
                break;
            case (int)ROOM.supermarket1_3:
                fileName = "Task_1_Subtask_3.csv";
                break;
            case (int)ROOM.supermarket2_1:
                fileName = "Task_2_Subtask_1.csv";
                break;
            case (int)ROOM.supermarket2_2:
                fileName = "Task_2_Subtask_2.csv";
                break;
            case (int)ROOM.supermarket2_3:
                fileName = "Task_2_Subtask_3.csv";
                break;
            case (int)ROOM.supermarket2_4:
                fileName = "Task_2_Subtask_4.csv";
                break;
            default:
                break;


        }

        fs = new FileStream(fileName, FileMode.OpenOrCreate);
        if (fs.Length == 0){
        try
        {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.Write("ID,GrabTime,PosTime,GrabErrorRate,PosErrorRate,ResetCounter,Sucessful,Snapping");
                }
            }
        finally
        {
            if (fs != null)
                fs.Dispose();
        }
    }
        return fs != null;
    }


    // Data has to be structured as long array like: [0] testID , [1] grabTime, [2] posTime, [3] grabErrorRate, [4] posErrorRate, [5] reset Counter, [6] Sucessful{0,1},[7] Snapping {0,1}
    public bool addData2CSVFile(int room, long[] data) {
        if (data.Length != 8) return false;
        initCSVFile(room);
        string line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7]);

        try
        {
            using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
            {
                writer.Write(line);
            }
        }
        finally
        {
            if (fs != null)
                fs.Dispose();
        }
           
        return false;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
