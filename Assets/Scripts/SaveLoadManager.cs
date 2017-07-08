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

	private static void Save (string data, string fileName)
	{
		File.WriteAllText(fileName,data);
	}

	public static void Save (Saveable obj)
	{
		Save (obj.GetSaveable(), obj.FullFileName());
	}

	private static string Load (string fileName)
	{
		return File.ReadAllText(fileName);
	}

	public static string Load (Saveable obj)
	{
		return Load (obj.FullFileName());
	}

	public static bool Delete (Saveable obj)
	{
		return Delete (obj.FullFileName()) ;
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