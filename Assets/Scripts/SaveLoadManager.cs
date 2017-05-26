using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;


public class SaveLoadManager : MonoBehaviour
{
    public static string MapsFolder;
    public static string InfoFolder ;
    public static string BricksFolder ;
    public static string FileExtension = ".dat";
    public static void PrepareFolder()
    {
        MapsFolder = Application.persistentDataPath + "/Maps/";
        BricksFolder = MapsFolder + "Bricks/";
        InfoFolder = MapsFolder + "Info/";
    }
    public static void CreateMapsFolder()
    {
        PrepareFolder();
        Directory.CreateDirectory(MapsFolder);
        Directory.CreateDirectory(InfoFolder);
        Directory.CreateDirectory(BricksFolder);
    }
    public static void SaveCurrentLego(string FileName)
    {
        //Save(AddNew.TheSet, FileName);
    }

    public static string[] FetchMapsInfoCodes()
    {
        PrepareFolder();
        return Directory.GetFiles(InfoFolder);
    }

    private static void Save(object obj, string fileName, string dir)
    {
        String fullFileName = dir + fileName + FileExtension;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fullFileName);
        bf.Serialize(file, obj);
        file.Close();
    }
    public static void Save(Info info,string fileName)
    {
        Save(info, fileName, InfoFolder);
    }
    public static void Save(Bricks bricks, string fileName)
    {
        Save(bricks, fileName, BricksFolder);
    }
    private static object Load(string fileName, string dirName)
    {
        string fullFileName = dirName + fileName + FileExtension;
        if (File.Exists(fullFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fullFileName, FileMode.Open);
            var m = bf.Deserialize(file);
            file.Close();
            return m;
        }
        return null;
    }
    public static Info LoadInfoFile(string fileName)
    {
        return (Info)Load(fileName, InfoFolder);
    }
    public static Bricks LoadBrickFile(string fileName)
    {
        return (Bricks)Load(fileName, BricksFolder);
    }

}
