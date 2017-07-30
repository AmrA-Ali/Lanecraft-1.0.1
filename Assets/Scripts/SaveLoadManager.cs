using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

namespace LC.SaveLoad
{
	public class SaveLoadManager : MonoBehaviour
	{
		public static bool FIRST_TIME;
		public static bool READY = false;
		
		void Awake(){
			Debug.Log("SaveLoadManager Awake...");
			READY = false;
			FIRST_TIME = IsFirstTime();
			Debug.Log("SaveLoadManager.FIRST_TIME: "+FIRST_TIME);
			if (FIRST_TIME){
				CreateFiles();
			}
			READY = true;
		}

		public static bool IsFirstTime(){
			FillFileNames();
			return !File.Exists(FILE.BUILD);
		}

		public static void FillFileNames ()
		{
			FILE.D.MAPS = Application.persistentDataPath + "/Maps/";
			FILE.D.BRICKS = FILE.D.MAPS + "Bricks/";
			FILE.D.INFO = FILE.D.MAPS + "Info/";

			FILE.BUILD = Application.persistentDataPath + "/Build.ini";
			FILE.PLAYER = Application.persistentDataPath + "/Player.ini";
			FILE.TEMP = "TEMP";
		}

		public static void CreateFiles ()
		{
			Directory.CreateDirectory (FILE.D.MAPS);
			Directory.CreateDirectory (FILE.D.INFO);
			Directory.CreateDirectory (FILE.D.BRICKS);
			Save(new Build());
			Save(new Player.PlayerData());
			new Map {info = {code = FILE.TEMP}}.SaveAsIs();
		}

		public static string[] FetchMapsInfoCodes ()
		{
			return Directory.GetFiles (FILE.D.INFO);
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

		public static bool Delete (Saveable obj)
		{
			return Delete (obj.FullFileName()) ;
		}

		private class Build:Saveable
		{
			private const string BUILD_FILE_MESSAGE =
			"totallyCalculated Games\n\nLanecraft v1.0.0\n©Everything Reserved";
			
			public string FullFileName(){
				return FILE.BUILD;
			}
			public string FileName(){
				return "NO NEED";
			}
			public string GetSaveable(){
				return ""+BUILD_FILE_MESSAGE;
			}
			public void SetSaveable(string s){
				//no need now
			}

		}
	}
	public static class FILE
	{ 
		public static string EXT = ".dat";//Files extenstion
		public static string BUILD;
		public static string PLAYER;
		public static string TEMP;
		
		public static class D //Directories
		{
			public static string MAPS;
			public static string INFO;
			public static string BRICKS;
		}
	}

	public interface Saveable{
		string FullFileName();
		string FileName();
		string GetSaveable();
		void SetSaveable(string s);
	}
}