using UnityEngine;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using LC.SaveLoad;

public class Map
{
	#region variables

	public Info info;
	public Bricks bricks;
	public static Map curr;
	public static GameObject mapParent;
	public bool isOffline;
	public bool isOnline;
	public bool isMine;
	public bool isShared;
	public string uploadId;
	private List<GameObject> TheSet;
	private static GameObject[] Shapes = Resources.LoadAll<GameObject> ("Prefabs/Shapes");
    private static GameObject[] Obstacles = Resources.LoadAll<GameObject>("Prefabs/Obstacles");
    private static GameObject FinishLinePrefab = Resources.Load<GameObject> ("Prefabs/YOUJUSTWON");
    public int currBrick;
	#endregion

	#region ctor

	public Map ()
	{
		TheSet = new List<GameObject> ();
		info = new Info ();
		bricks = new Bricks ();
        currBrick = 0;
	}

	public static bool operator ==(Map a,Map b){
		return a.info.code.Equals(b.info.code);
	}
	
	public static bool operator !=(Map a,Map b){
		return !(a==b);
	}
	#endregion

	public void UpdateInfo(){
		SaveLoadManager.Save(info);
	}
	public bool Delete ()
	{
		return SaveLoadManager.Delete (info) && SaveLoadManager.Delete (bricks);
	}

	#region online
	public void Upload(){
		this.info.isOnline = true;
		bricks.code = info.code;
		Online.Upload(bricks);
		Online.AddToMapsCollection(this.info);
		this.UpdateInfo();
	}

	public void Download(){
		Online.Download(this.uploadId,this);	
	}

	#endregion

	#region loading

	//Save Map without doing Calculations
	public void SaveAsIs(){
		bricks.code= info.code;
		
		SaveLoadManager.Save (bricks);
		SaveLoadManager.Save (info);
	}

	//Normal saving with calculations
	public void Save ()
	{
		DoCalculations ();
		SaveLoadManager.Save (bricks);
		SaveLoadManager.Save (info);
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
		bricks.code = info.code;
		bricks.SetSaveable(SaveLoadManager.Load (bricks));
	}

	public static Map[] CollectionToMaps (Dictionary<string,object> dict)
	{
		List<Map> maps = new List<Map> ();

		List<object> l = (List<object>)dict ["maps"];
		foreach (object o in l) {
			Dictionary<string,object> d = (Dictionary<string,object>)o;

			Map m = new Map ();
			m.uploadId = (string)d ["id"];
			m.info.SetSaveable((string)d ["info"]);

			m.isOffline = false;
			m.isMine = m.info.creator.Equals (Player.DATA.Creator ());
			maps.Add (m);
		}
		return maps.ToArray ();
	}

	public static Map[] FetchMapsInfoOffline ()
	{
		List<Map> maps = new List<Map> ();
		foreach (var code in SaveLoadManager.FetchMapsInfoCodes()) {
			Map m = new Map ();
			m.info.code = code.FilterFileExtension (FILE.EXT);
			m.info.SetSaveable(SaveLoadManager.Load (m.info));

			m.isOffline = true;
			m.isMine = m.info.creator.Equals (Player.DATA.Creator ());
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

    private GameObject AddObstacle(GameObject mygb, bool building = false)
    {
        GameObject gb2;
        Transform trans;

        trans = TheSet[currBrick].transform.GetChild(3);
        gb2 = MonoBehaviour.Instantiate(mygb, trans.position, trans.rotation) as GameObject;

        gb2.name = mygb.name;
        CreateMapParent();
        gb2.transform.SetParent(mapParent.transform);
        TheSet.Add(gb2);
        if (building)
        {
            bricks.list.Add(gb2.name);
            Camera.main.UpdateCamera(TheSet[++currBrick].transform.GetChild(0));
        }
        return gb2;
    }

    public GameObject AddObstacle(string objectName, bool building = false)
    {
        foreach (var e in Obstacles)
        {
            if (objectName == e.name)
            {
                return AddObstacle(e, building);
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
		info.SetDateNow();
	}

	void CalculateCreator ()
	{
		info.creator = Player.DATA.Creator ();
	}

	void CalculateCount ()
	{
		info.brickCount = bricks.list.Count;
	}

	private void CalculateCode ()
	{
		info.code = Player.DATA.id.Substring (20);
		info.code += info.brickCount.ToString ("X");
		info.code += (info.dateCreated.Year % 100).ToString("X");

		info.code += info.dateCreated.DayOfYear.ToString ("X");
		info.code += info.dateCreated.Hour.ToString ("X");
		info.code += info.dateCreated.Minute.ToString ("X");
		info.code += info.dateCreated.Second.ToString ("X");
		info.code += info.dateCreated.Millisecond.ToString ("X").Substring (0, 1);

		bricks.code = info.code;
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

public class Bricks:Saveable
{
	public List<string> list;
	public string code;

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
	public Bricks(int[] a){
		list = new List<string>(ToString(a));
	}

	public string FullFileName(){
		return FILE.D.BRICKS + FileName() + FILE.EXT;
	}
	public string FileName(){
		return code;
	}

	public string GetSaveable(){
		return string.Join("!", Array.ConvertAll(ToInt(),i => i.ToString()));
	}
	public void SetSaveable(string s){
		int[] a = Array.ConvertAll(s.Split('!'),x => int.Parse(x));
		list = new List<string>(ToString(a));
	}

	public int[] ToInt(){
		return list.ConvertAll(
			new Converter<string,int>(StringToInt)).ToArray();
	}

	public string[] ToString(int[] a){
		return Array.ConvertAll(a,
			new Converter<int,string>(IntToString));
	}
	private static int StringToInt(string s){
		var THE_DICT = new Dictionary<string,int> (){
			{"Line",1},
			{"TurnRight",2},
			{"TurnLeft",3},
			{"CurveUp",4},
			{"CurveDown",5},
			{"TightRight",6},
            {"FullWall",50},
            {"RightWall",51},
            {"LeftWall",52}

        };
		return THE_DICT[s];
	}

	private static string IntToString(int i){
		var THE_DICT = new Dictionary<int,string> (){
			{1,"Line"},
			{2,"TurnRight"},
			{3,"TurnLeft"},
			{4,"CurveUp"},
			{5,"CurveDown"},
			{6,"TightRight"}
			
		};
		return THE_DICT[i];
	}
}

public class Info:Saveable
{
	public string code,name,creator;
	public OurDate dateCreated, dateUpdated;
	public int difficulty = 0,highestScore = 0;
	public bool isOnline;
	public int brickCount;
	public OurVector3 minBound, maxBound, center;
	public Stats statistics;

	public Info ()
	{
		name = "OMG";
		creator = "ALi";
		minBound = new OurVector3 ();
		maxBound = new OurVector3 ();
		statistics = new Stats ();
	}

	public string GetSaveable(){
		return ""+
		code+"!"+
		name+"!"+
		creator+"!"+
		dateCreated.ToString(true)+"!"+
		dateUpdated.ToString(true)+"!"+
		difficulty.ToString()+"!"+
		highestScore.ToString()+"!"+
		isOnline.ToString()+"!"+
		brickCount.ToString()+"!"+
		minBound.ToString()+"!"+
		maxBound.ToString()+"!"+
		center.ToString()+"!"+
		statistics.ToString();
	}

	public void SetSaveable(string s){
		string[] f = s.Split('!');
		code=f[0];
		name=f[1];
		creator=f[2];
		dateCreated = new OurDate(f[3]);
		dateUpdated = new OurDate(f[4]);
		difficulty = int.Parse(f[5]);
		highestScore = int.Parse(f[6]);
		isOnline = bool.Parse(f[7]);
		brickCount = int.Parse(f[8]);
		minBound = new OurVector3(f[9]);
		maxBound = new OurVector3(f[10]);
		center = new OurVector3(f[11]);
		statistics = new Stats(f[12]);
	}

	public string FullFileName(){
		return FILE.D.INFO + FileName() + FILE.EXT;
	}
	public string FileName(){
		return code;
	}
	public void SetDateNow(){
		dateCreated = new OurDate(DateTime.Now);
		dateUpdated = dateCreated;
	}

	public class OurDate
	{
		public int Year,DayOfYear,Hour,Minute,Second,Millisecond;

		public OurDate(DateTime t){
			Year = t.Year;
			DayOfYear = t.DayOfYear;
			Hour = t.Hour;
			Minute = t.Minute;
			Second = t.Second;
			Millisecond = t.Millisecond;
		}
		public OurDate(string t){
			int[] f = Array.ConvertAll(t.Split(':'), s => int.Parse(s));
			Year = f[0];
			DayOfYear = f[1];
			Hour = f[2];
			Minute = f[3];
			Second = f[4];
			Millisecond = f[5];
		}

		public override string ToString(){
			return "" + DayOfYear + "/" + Year + " - " + Hour + ":" + Minute;
		}
		public  string ToString(bool f){
			return ""+Year+":"+DayOfYear+":"+Hour+":"+Minute+":"+Second+":"+Millisecond;
		}
	}

	public class Stats
	{
		public int turnRights, turnLefts, curveUps, curveDowns, lines;
		public int obstacleCount;

		public Stats (){
		}

		public Stats(string s){
			int[] f = Array.ConvertAll(s.Split(':'), x => int.Parse(x));
			turnRights = f[0];
			turnLefts = f[1];
			curveUps = f[2];
			curveDowns = f[3];
			lines = f[4];
			obstacleCount = f[5];
		}

		public override string ToString(){
			return ""+ turnRights+":"+ turnLefts+":"+curveUps+":"+ curveDowns+":"+lines+":"+ obstacleCount;
		}
	}

	public class OurVector3
	{
		public float x, y, z;

		public OurVector3 ()
		{
		}

		public OurVector3 (string s)
		{
			float[] f = Array.ConvertAll(s.Split(':'), x => float.Parse(x));
			x=f[0];
			y=f[1];
			z=f[2];
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
		public override string ToString(){
			return ""+x+":"+y+":"+z;
		}
	}
}