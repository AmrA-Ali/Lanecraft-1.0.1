using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour
{
	public static string MapsFolder;
	public static string InfoFolder;
	public static string BricksFolder;
	public static string FileExtension = ".dat";

	public static void PrepareFolder ()
	{
		MapsFolder = Application.persistentDataPath + "/Maps/";
		BricksFolder = MapsFolder + "Bricks/";
		InfoFolder = MapsFolder + "Info/";
	}

	public static void CreateMapsFolder ()
	{
		PrepareFolder ();
		Directory.CreateDirectory (MapsFolder);
		Directory.CreateDirectory (InfoFolder);
		Directory.CreateDirectory (BricksFolder);
	}

	public static void SaveCurrentLego (string FileName)
	{
		//Save(AddNew.TheSet, FileName);
	}

	public static string[] FetchMapsInfoCodes ()
	{
		PrepareFolder ();
		return Directory.GetFiles (InfoFolder);
	}

	private static void Save (object obj, string fileName)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (fileName);
		bf.Serialize (file, obj);
		file.Close ();
	}

	public static void Save (Map map)
	{
		string fileName = map.info.code + FileExtension;
		Save (map.info, InfoFolder + fileName);
		Save (map.bricks, BricksFolder + fileName);
	}

	private static object Load (string fileName, string dirName)
	{
		string fullFileName = dirName + fileName + FileExtension;
		if (File.Exists (fullFileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (fullFileName, FileMode.Open);
			var m = bf.Deserialize (file);
			file.Close ();
			return m;
		}
		return null;
	}

	public static Info LoadInfoFile (string fileName)
	{
		return (Info)Load (fileName, InfoFolder);
	}

	public static Bricks LoadBrickFile (string fileName)
	{
		return (Bricks)Load (fileName, BricksFolder);
	}

	public static bool Delete (Map map)
	{
		string fileName = map.info.code + FileExtension;
		return Delete (InfoFolder + fileName) && Delete (BricksFolder + fileName);
	}

	public static bool Delete (string fileName)
	{
		try {
			File.Delete (fileName);
		} catch (Exception e) {
			print (e);
			return false;
		}
		return true;
	}
}