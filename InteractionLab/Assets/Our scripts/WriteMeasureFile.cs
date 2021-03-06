﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class WriteMeasureFile
{
    private string path;
    private List<string> tips = new List<string>();
    private List<int> ButtonIds = new List<int>();
    private List<int> IsTexture = new List<int>();
    private List<int> heights = new List<int>();
    FileStream fs;
    enum ROOM { learn, supermarket1_1, supermarket1_2, supermarket1_3, supermarket2_1, supermarket2_2, supermarket2_3, supermarket2_4, supermarket2_5 };
    enum Method { CLOSE_SIMPLE, CLOSE_DIST, CLOSE_ROD, FAR_RAYCAST, FAR_INDIRECT_RAY };

    // Use this for initialization
    public WriteMeasureFile()
    {
    }
    private void initContraintedMethodCSVFile(int room, int method)
    {

        switch (room)
        {
           case (int)ROOM.supermarket2_1:
                path += "Task_2_Subtask_1.csv";
                break;
            case (int)ROOM.supermarket2_2:
                path += "Task_2_Subtask_2.csv";
                break;
            case (int)ROOM.supermarket2_3:
                path += "Task_2_Subtask_3.csv";
                break;
            case (int)ROOM.supermarket2_4:
                path += "Task_2_Subtask_4.csv";
                break;
            case (int)ROOM.supermarket2_5:
                path += "Task_2_Subtask_5.csv";
                break;
            default:
                break;


        }


        fs = new FileStream(path, FileMode.OpenOrCreate);
        if (fs.Length == 0)
        {
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



    public bool addChoosenMethodData2CSVFile(long[] data, int room)
    {
        if (data.Length != 7) return false;

        switch (room) {
           case(int)ROOM.supermarket1_1:
                path += "Task_1_Subtask_1.csv";
        break;
            case (int)ROOM.supermarket1_2:
                path += "Task_1_Subtask_2.csv";
        break;
            case (int)ROOM.supermarket1_3:
                path += "Task_1_Subtask_3.csv";
        break;
        default:
        break;
    }
    

        fs = new FileStream(path, FileMode.OpenOrCreate);
        if (fs.Length == 0)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.WriteLine("ID,  time Touch Grab, time Proxmitiy, time Wand Grab, time Raycast, time Extend Ray , Sucessful Method");
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


    public bool addLearningData2CSVFile(long[] data)
    {
        if (data.Length != 6) return false;

        path += "Learning.csv";

        fs = new FileStream(path, FileMode.OpenOrCreate);
        if (fs.Length == 0)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.Default))
                {
                    writer.WriteLine("ID, time Touch Grab, time Proxmitiy, time Wand Grab, time Raycast, time Extend Ray");
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
    public bool addContrainedMethodData2CSVFile(int room, int method, long[] data)
    {
        if (data.Length != 7) return false;
        initContraintedMethodCSVFile(room, method);
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

    // Use this for initialization
    public void initSelfteaching()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(@"SelfTeaching.csv"))
        {
            if (file != null)
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split(';');

                    tips.Add(values[1]);
                    ButtonIds.Add(System.Convert.ToInt32(values[2]));
                    IsTexture.Add(System.Convert.ToInt32(values[3]));
                    heights.Add(System.Convert.ToInt32(values[4]));
                }
                file.Close();
            }
        }
    }

    public void initTasks()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(@"Tasks.csv"))
        {
            if (file != null)
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split(';');

                    tips.Add(values[0]);
                    heights.Add(System.Convert.ToInt32(values[1]));
                }
                file.Close();
            }
        }
    }

    public void initBoard()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(@"supermarket_boards.csv"))
        {
            if (file != null)
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] values = line.Split(';');

                    tips.Add(values[0]);
                    heights.Add(System.Convert.ToInt32(values[1]));
                }
                file.Close();
            }
        }
    }


    public List<string> getTips()
    {
        return tips;
    }
    public List<int> getButtonIds()
    {
        return ButtonIds;
    }
    public List<int> getHeights()
    {
        return heights;
    }
    public List<int> getIsTextures()
    {
        return IsTexture;
    }


}
