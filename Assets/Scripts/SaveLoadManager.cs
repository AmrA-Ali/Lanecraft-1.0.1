using UnityEngine;
using System;
using System.IO;

namespace LC.SaveLoad
{
	public class SaveLoadManager : MonoBehaviour
	{
		public static bool FirstTime;
		
		public static void GetReady(Action callBack)
		{
			FirstTime = IsFirstTime();
			if (FirstTime){
				CreateFiles();
			}
			callBack();
		}

		private static bool IsFirstTime(){
			FillFileNames();
			return !File.Exists(FILE.Build);
		}

		private static void FillFileNames ()
		{
			FILE.D.Maps = Application.persistentDataPath + "/Maps/";
			FILE.D.Bricks = FILE.D.Maps + "Bricks/";
			FILE.D.Info = FILE.D.Maps + "Info/";

			FILE.Build = Application.persistentDataPath + "/Build.ini";
			FILE.Player = Application.persistentDataPath + "/Player.ini";
			FILE.Temp = "TEMP";
		}

		private static void CreateFiles ()
		{	
			Debug.Log("SaveLoadManager.FIRST_TIME: "+FirstTime);
			Directory.CreateDirectory (FILE.D.Maps);
			Directory.CreateDirectory (FILE.D.Info);
			Directory.CreateDirectory (FILE.D.Bricks);
			Save(new Build());
			Save(new Player.PlayerData());
			new Map {Info = {Code = FILE.Temp}}.SaveAsIs();
		}

		public static string[] FetchMapsInfoCodes ()
		{
			return Directory.GetFiles (FILE.D.Info);
		}

		private static void Save (string data, string fileName)
		{
			File.WriteAllText(fileName,data);
		}

		public static void Save (ISaveable obj)
		{
			Save (obj.GetSaveable(), obj.FullFileName());
		}

		private static string Load (string fileName)
		{
			return File.ReadAllText(fileName);
		}

		public static string Load (ISaveable obj)
		{
			return Load (obj.FullFileName());
		}

		private static bool Delete (string fileName)
		{
			try {
				File.Delete (fileName);
			} catch (Exception e) {
				print (e);
				return false;
			}
			return true;
		}

		public static bool Delete (ISaveable obj)
		{
			return Delete (obj.FullFileName()) ;
		}

		private class Build:ISaveable
		{
			private const string BuildFileMessage =
			"totallyCalculated Games\n\nLanecraft v1.0.0\n©Everything Reserved";
			
			public string FullFileName(){
				return FILE.Build;
			}
			public string FileName(){
				return "NO NEED";
			}
			public string GetSaveable(){
				return ""+BuildFileMessage;
			}
			public void SetSaveable(string s){
				//no need now
			}

		}

	}
	public static class FILE
	{ 
		public static string Ext = ".dat";//Files extenstion
		public static string Build;
		public static string Player;
		public static string Temp;
		
		public static class D //Directories
		{
			public static string Maps;
			public static string Info;
			public static string Bricks;
		}
	}

	public interface ISaveable{
		string FullFileName();
		string FileName();
		string GetSaveable();
		void SetSaveable(string s);
	}
}