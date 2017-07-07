using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WriteMeasureFile {
    private string path;
    FileStream fs;
    enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };

    // Use this for initialization
    public WriteMeasureFile() {
    }
    private void initCSVFile(int room, int method) {
        
        switch (room)
        {
            case (int)ROOM.learn:
                path += "Learning";
                break;
            case (int)ROOM.supermarket1_1:
                path += "Task_1_Subtask_1";
                break;
            case (int)ROOM.supermarket1_2:
                path += "Task_1_Subtask_2";
                break;
            case (int)ROOM.supermarket1_3:
                path += "Task_1_Subtask_3";
                break;
            case (int)ROOM.supermarket2_1:
                path += "Task_2_Subtask_1";
                break;
            case (int)ROOM.supermarket2_2:
                path += "Task_2_Subtask_2";
                break;
            case (int)ROOM.supermarket2_3:
                path += "Task_2_Subtask_3";
                break;
            case (int)ROOM.supermarket2_4:
                path += "Task_2_Subtask_4";
                break;
            default:
                break;


        }

        switch (method)
        {
            case (int)Method.CLOSE_SIMPLE:
                path += "_CLoseSimple.csv";
                break;
                case(int)Method.CLOSE_DIST:
                     path += "_CLoseDist.csv";
                break;
            case (int)Method.CLOSE_ROD:
                path += "_CLoseRod.csv";
                break;
            case (int)Method.FAR_RAYCAST:
                path += "_FarRaycast.csv";
                break;
            case (int)Method.FAR_INDIRECT_RAY:
                path += "_FarIndirectRay.csv";
                break;
            default:
                break;

        }


        fs = new FileStream(path, FileMode.OpenOrCreate);
        if (fs.Length == 0){
        try
        {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.WriteLine("ID,GrabTime,PosTime,errorRate,WrongSelectedRate,Sucessful,Snapping");
                    writer.Close();
                }
            }
        finally
        {
                if (fs != null)
                    fs.Dispose();

            }
    }
                fs.Close();
    }

    public bool addLearningData2CSVFile(long[] data) {
        if (data.Length != 6) return false;

        path += "Learning.csv";

        fs = new FileStream(path, FileMode.OpenOrCreate);
        if (fs.Length == 0)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.WriteLine("ID, time Simple, time Distance, time Rod, time Raycast, time Indirect Ray");
                    writer.Close();
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

            }
        }
        fs.Close();

        string line = string.Format("{0},{1},{2},{3},{4},{5}", data[0].ToString(), data[1].ToString(), data[2].ToString(), data[3].ToString(), data[4].ToString(), data[5].ToString());

        using (var file = File.Open(path, FileMode.Append, FileAccess.Write))
        {
            using (var writer = new StreamWriter(file))
            {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine(line);
                writer.Close();
            }
            file.Close();

        }
        return true;

    }
    // Data has to be structured as long array like: [0] testID , [1] grabTime, [2] posTime, [3] errorRate, [4]WrongSelectedRate , [5] Sucessful{0,1}, [6] Snapping {0,1}
    public bool addData2CSVFile(int room, int method, long[] data) {
        if (data.Length != 7) return false;
        initCSVFile(room, method);
        string line = string.Format("{0},{1},{2},{3},{4},{5},{6}", data[0].ToString(), data[1].ToString(), data[2].ToString(), data[3].ToString(), data[4].ToString(), data[5].ToString(), data[6].ToString());
       
        using (var file = File.Open(path, FileMode.Append, FileAccess.Write))
        {
            using (var writer = new StreamWriter(file))
            {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine(line);
                writer.Close();
            }
            file.Close();

        }

        return true;
    }
	// Update is called once per frame
	void Update () {
		
	}

   

}
