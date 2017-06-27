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
		Save (map.info, map.FileNameInfo());
		Save (map.bricks, map.FileNameBricks());
	}

	private static object Load (string fileName)
	{
		if (File.Exists (fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (fileName, FileMode.Open);
			
			var m = bf.Deserialize (file);
			file.Close ();
			return m;
		}
		return null;
	}

	public static Info LoadInfoFile (Map map)
	{
		return (Info)Load (map.FileNameInfo());
	}

	public static Bricks LoadBrickFile (Map map)
	{
		return (Bricks)Load (map.FileNameBricks());
	}

	public static bool Delete (Map map)
	{
		return Delete (map.FileNameInfo()) && Delete (map.FileNameBricks());
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