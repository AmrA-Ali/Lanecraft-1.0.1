using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class InstallManager : MonoBehaviour {
    private const string BuildFileMessage =
        "totallyCalculated Games\n\nLanecraft v1.0.0\n©Everything Reserved";
    private static string BuildFileName;

    void Awake()
    {
        BuildFileName = Application.persistentDataPath + "/Build.ini";
        //SaveLoadManager.PrepareFolder();//brute
        if (!File.Exists(BuildFileName)) //first time running game, create required folders
        {
            CreateBuildFile(); 
            SaveLoadManager.CreateMapsFolder();
        }
	}
    private void CreateBuildFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(BuildFileName);
        bf.Serialize(file, BuildFileMessage);
        file.Close();
    }
    //read and print content of Build file, for testing
    void test()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(BuildFileName, FileMode.Open);
        string m = (string)bf.Deserialize(file);
        file.Close();
        print(m);
    }
}
