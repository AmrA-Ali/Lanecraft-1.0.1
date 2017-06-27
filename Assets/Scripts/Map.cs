using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Map
{
	#region variables

	public Info info;
	public Bricks bricks;
	public static Map curr;
	public static GameObject mapParent;
	public bool isOffline;
	public bool isMine;
	public bool isShared;
	public string uploadId;
	private List<GameObject> TheSet;
	private static GameObject[] Shapes = Resources.LoadAll<GameObject> ("Prefabs/Shapes");
	private static GameObject FinishLinePrefab = Resources.Load<GameObject> ("Prefabs/YOUJUSTWON");

	#endregion

	#region ctor

	public Map ()
	{
		TheSet = new List<GameObject> ();
		info = new Info ();
		bricks = new Bricks ();
	}

	#endregion

	public string FileNameInfo ()
	{
		return SaveLoadManager.InfoFolder + info.code + SaveLoadManager.FileExtension;
	}

	public string FileNameBricks ()
	{
		return SaveLoadManager.BricksFolder + info.code + SaveLoadManager.FileExtension;
	}

	public void Delete ()
	{
		SaveLoadManager.Delete (this);
	}

	#region loading

	public void Save ()
	{
		DoCalculations ();
		SaveLoadManager.Save (this);
	}

	public void CreateMapParent ()
	{
		if (mapParent == null) {
			mapParent = new GameObject ();
			mapParent.name = "Track";
		}
	}

	public void FetchBricks ()
	{
		bricks = SaveLoadManager.LoadBrickFile (this);
	}

	public static Map[] FetchMapsInfoOnline (Dictionary<string,object> dict)
	{
		List<Map> maps = new List<Map> ();

		List<object> l = (List<object>)dict ["maps"];
		foreach (object o in l) {
			Dictionary<string,object> d = (Dictionary<string,object>)o;
			string uploadId = (string)d ["id"];
			Info info = JsonUtility.FromJson <Info> ((string)d ["info"]);

			Map m = new Map ();
			m.uploadId = uploadId;
			m.info = info;

			m.isOffline = false;
			m.isMine = m.info.creator.Equals (Auth.Creator ());
			maps.Add (m);
		}
		return maps.ToArray ();
	}

	public static Map[] FetchMapsInfoOffline ()
	{
		List<Map> maps = new List<Map> ();
		foreach (var code in SaveLoadManager.FetchMapsInfoCodes()) {
			Map m = new Map ();
			m.info.code = code.FilterFileExtension (SaveLoadManager.FileExtension);
			m.info = SaveLoadManager.LoadInfoFile (m);

			m.isOffline = true;
			m.isMine = m.info.creator.Equals (Auth.Creator ());
			maps.Add (m);
		}
		return maps.ToArray ();
	}

	#endregion

	#region brickbuilding

	public void Build ()
	{
		FetchBricks ();
		foreach (string brickName in bricks.list) {
			AddBrick (brickName);
		}
		AddFinishLine ();
	}

	public void RemoveLastObject ()
	{
		if (TheSet.Count >= 1) {
			if (TheSet.Count > 1)
				Camera.main.UpdateCamera (TheSet [TheSet.Count - 2].transform.GetChild (0));
			MonoBehaviour.Destroy (TheSet [TheSet.Count - 1]);
			TheSet.RemoveAt (TheSet.Count - 1);
			bricks.list.RemoveAt (bricks.list.Count - 1);
		}
	}

	private GameObject AddBrick (GameObject mygb, bool building = false)
	{
		GameObject gb2;
		Transform trans;
		if (TheSet.Count == 0) {
			gb2 = MonoBehaviour.Instantiate (mygb) as GameObject;
			gb2.transform.position = new Vector3 (0, 0, 0);
		} else {
			trans = TheSet [TheSet.Count - 1].transform.GetChild (1);
			gb2 = MonoBehaviour.Instantiate (mygb, trans.position, trans.rotation) as GameObject;
		}
		gb2.name = mygb.name;
		CreateMapParent ();
		gb2.transform.SetParent (mapParent.transform);
		TheSet.Add (gb2);
		if (building) {
			bricks.list.Add (gb2.name);
			Camera.main.UpdateCamera (gb2.transform.GetChild (0));
		}
		return gb2;
	}

	public GameObject AddBrick (string objectName, bool building = false)
	{
		foreach (var e in Shapes) {
			if (objectName == e.name) {
				return AddBrick (e, building);
			}
		}
		return null;
	}

	public GameObject AddFinishLine ()
	{
		var temp = AddBrick (FinishLinePrefab);
		ClearSet ();
		return temp;
	}

	private void ClearSet ()
	{
		TheSet.Clear ();
	}

	#endregion

	#region calculations

	public void DoCalculations ()
	{
		CalculateCreatedDate ();
		CalculateCreator ();
		CalculateCount ();
		CalculateBounds ();
		CalculateCode ();
	}

	void CalculateCreatedDate ()
	{
		info.dateCreated = DateTime.Now;
	}

	void CalculateCreator ()
	{
		info.creator = Auth.Creator ();
	}

	void CalculateCount ()
	{
		info.brickCount = bricks.list.Count;
	}

	private void CalculateCode ()
	{
		info.code = Auth.UID.Substring (20);
		info.code += info.brickCount.ToString ("X");
		info.code += info.dateCreated.DayOfYear.ToString ("X");
		info.code += info.dateCreated.Hour.ToString ("X");
		info.code += info.dateCreated.Minute.ToString ("X");
		info.code += info.dateCreated.Second.ToString ("X");
		info.code += info.dateCreated.Millisecond.ToString ("X").Substring (0, 1);
	}

	public void CalculateBounds ()
	{
		Vector3 trans;
		for (int i = 0; i < TheSet.Count; i++) {
			trans = TheSet [i].transform.position;
			if (trans.x < info.minBound.x)
				info.minBound.x = trans.x;	
			if (trans.x > info.maxBound.x)
				info.maxBound.x = trans.x;

			if (trans.y < info.minBound.y)
				info.minBound.y = trans.y;
			if (trans.y > info.maxBound.y)
				info.maxBound.y = trans.y;

			if (trans.z < info.minBound.z)
				info.minBound.z = trans.z;
			if (trans.z > info.maxBound.z)
				info.maxBound.z = trans.z;
		}
		info.center = new Info.OurVector3 ((info.maxBound.get () + info.minBound.get ()) / 2);

	}

	#endregion
}


[Serializable]
public class Bricks
{
	public List<string> list;

	public Bricks ()
	{
		list = new List<string> ();
	}

	public Bricks (List<GameObject> TheSet)
	{
		for (int i = 0; i < TheSet.Count; i++) {
			list.Add (TheSet [i].name.Substring (0, TheSet [i].name.Length - "(Clone)".Length));
		}
	}
}


[Serializable]
public class Info
{
	public string creator;
	public OurVector3 minBound, maxBound, center;
	public DateTime dateCreated;
	public DateTime dateUpdated;
	public int brickCount;
	public Stats statistics;
	public int difficulty = 0;
	public int highestScore = 0;
	public string name;
	public string code;

	public Info ()
	{
		name = "OMG";
		creator = "ALi";
		minBound = new OurVector3 ();
		maxBound = new OurVector3 ();
		statistics = new Stats ();
	}

	[Serializable]
	public class Stats
	{
		public int turnRights, turnLefts, curveUps, curveDowns, lines;
		public int obstacleCount;
	}

	[Serializable]
	public class OurVector3
	{
		public float x, y, z;

		public OurVector3 ()
		{
		}

		public OurVector3 (Vector3 vec)
		{
			this.x = vec.x;
			this.y = vec.y;
			this.z = vec.z;
		}

		public Vector3 get ()
		{
			return new Vector3 (x, y, z);
		}
	}
}