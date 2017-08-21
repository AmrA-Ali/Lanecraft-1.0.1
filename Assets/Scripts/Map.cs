using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LC.MapUtls;
using LC.Online;
using LC.SaveLoad;
using UnityEngine;
using UnityEngine;
using UnityEngine.VR.WSA;
using Object = UnityEngine.Object;

public class Map : ISaveable
{
    public Info Info;
    public Bricks Bricks;
    public bool IsOffline;
    public bool IsOnline;
    public bool IsMine;
    public bool IsShared;
    public Slot Slot;
    public string Code;
    private readonly List<GameObject> _theSet,TheObs;

    private static readonly GameObject[] Shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    private static readonly GameObject FinishLinePrefab = Resources.Load<GameObject>("Prefabs/YOUJUSTWON");
    private static List<Map> OfflineMaps;
    private static List<Map> OnlineMaps;
    public static Map Curr;
    private static GameObject _mapParent;

    private Map()
    {
        Code = null;
        _theSet = new List<GameObject>();
        TheObs = new List<GameObject>();
        Info = new Info();
        Bricks = new Bricks();
    }

    public static void Default()
    {
        Curr = new Map();
    }

    public static void Temp()
    {
        Curr.Code = FILE.Temp;
        Curr._theSet.Clear();
        Curr.SetSaveable(SaveLoadManager.Load(Curr));
        Curr.Build(true);
    }

    public static void Save(string name, string code = null)
    {
        var m = Curr.Copy();
        m.Code = code;
        m.Info.Name = name;
        m.SaveOffline();
    }

    public static List<Map> GetMyMaps()
    {
        return OfflineMaps.FindAll(m => m.IsMine);
    }

    public static List<Map> GetOfflineMaps()
    {
        return OfflineMaps.FindAll(m => !m.IsMine);
    }

    public static List<Map> GetOnlineMaps()
    {
        return OnlineMaps;
    }

    private Map Copy()
    {
        return new Map
        {
            Code = Code,
            Info = new Info
            {
                Name = Info.Name,
                BrickCount = Info.BrickCount,
                Center = Info.Center,
                Creator = Info.Creator,
                DateCreated = Info.DateCreated,
                DateUpdated = Info.DateUpdated,
                Difficulty = Info.Difficulty,
                HighestScore = Info.HighestScore,
                MaxBound = Info.MaxBound,
                MinBound = Info.MinBound,
                Statistics = Info.Statistics
            },
            Bricks = new Bricks
            {
                List = Bricks.List
            }
        };
    }

    protected bool Equals(Map other)
    {
        return Equals(Info, other.Info);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Map) obj);
    }

    public override int GetHashCode()
    {
        return (Info != null ? Info.GetHashCode() : 0);
    }


    public static bool operator ==(Map a, Map b)
    {
        try
        {
            var x = a.Code.Equals(b.Code);
            return x;
        }
        catch (Exception e)
        {
            return (object) a == null && (object) b == null;
        }
    }

    public static bool operator !=(Map a, Map b)
    {
        return !(a == b);
    }

    public static void RefreshOfflineMaps()
    {
        OfflineMaps = LoadOfflineMaps();
    }

    public static void GetReady(Action callBack)
    {
        RefreshOfflineMaps();
        Online.GetMaps((onMaps) =>
        {
            if (onMaps != null)
                OnlineMaps = onMaps;
            callBack();
        });
    }

    public bool Delete()
    {
        return SaveLoadManager.Delete(this);
    }

    public void Upload(Action cb)
    {
        if (IsShared)
        {
            cb();
            return;
        }
        Online.Upload(this, () =>
        {
            SaveOffline();
            cb();
        });
    }

    //Normal saving with calculations
    public void SaveOffline()
    {
        SaveLoadManager.Save(this);
    }

    private static Map LoadFromOffline(string code)
    {
        var m = new Map {Code = code, IsOffline = true};
        m.SetSaveable(SaveLoadManager.Load(m));
        m.IsMine = m.Info.Creator.Equals(Player.Data.Creator());
        return m;
    }


    public GameObject MapParent()
    {
        try
        {
            var unused = _mapParent.activeInHierarchy; //Any test metod to see if it's there
        }
        catch (Exception)
        {
            _mapParent = new GameObject {name = "Track"};
        }
        return _mapParent;
    }

    public static Map LoadFromOnline(string s)
    {
        var m = new Map {IsOnline = true, IsMine = false};
        m.SetSaveable(s);
        return m;
    }

    private static List<Map> LoadOfflineMaps()
    {
        var maps = new List<Map>();
        foreach (var code in SaveLoadManager.AvailableMapsFiles())
        {
            if (code.Equals(FILE.Temp)) continue;
            Debug.Log("Map.FetchMapsInfoOffline.code: " + code);
            maps.Add(LoadFromOffline(code));
        }
        return maps;
    }

    public void Build(bool building = false)
    {
        foreach (var brickName in Bricks.List)
        {
            if (brickName[0] != '_')
            { AddBrick(brickName); }
            else AddObstacle(brickName.Remove(0,1));
        }
        if (!building)
    {
            AddBrick(FinishLinePrefab);
    ClearSet();
    }
    }

  
	public void RemoveLastBrick()
	{
		if (TheSet.Count >= 1) {
			if (TheSet.Count > 1)
			Camera.main.UpdateCamera (TheSet [TheSet.Count - 2].transform.GetChild (0));
            if (TheSet.Count == TheObs.Count)
                RemoveLastObstacle();
            MonoBehaviour.Destroy (TheSet [TheSet.Count - 1]);
            
			TheSet.RemoveAt (TheSet.Count - 1);
			bricks.list.RemoveAt (bricks.list.Count - 1);
    SaveOffline();
    }
	}
    public void RemoveLastObstacle()
    {
        if (TheObs.Count >= 1)
        {
            if (TheObs.Count > 1)
                Camera.main.UpdateCamera(TheSet[TheObs.Count - 2].transform.GetChild(0));
            MonoBehaviour.Destroy(TheObs[TheObs.Count - 1]);
            TheObs.RemoveAt(TheObs.Count - 1);
            bricks.list.RemoveAt(bricks.list.Count - 1);
    SaveOffline();
        }
    }

    private void AddBrick(GameObject mygb, bool building = false)
    {
        GameObject gb2;
        if (_theSet.Count == 0)
        {
            gb2 = Object.Instantiate(mygb);
            gb2.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            var trans = _theSet[_theSet.Count - 1].transform.GetChild(1);
            gb2 = Object.Instantiate(mygb, trans.position, trans.rotation);
        }
        gb2.name = mygb.name;
        gb2.transform.SetParent(MapParent().transform);
        _theSet.Add(gb2);
        if (!building) return;
        Bricks.List.Add(gb2.name);
        Camera.main.UpdateCamera(gb2.transform.GetChild(0));
    }

    public void AddBrick(string objectName, bool building = false)
    {
        foreach (var e in Shapes)
        {
            if (objectName != e.name) continue;
            AddBrick(e, building);
            if (building) SaveOffline();
            break;
        }
    }

   
    private GameObject AddObstacle(GameObject mygb, bool building = false)
    {
        GameObject gb2;
        Transform trans;
        if (TheObs.Count < TheSet.Count)
        {
            trans = TheSet[TheObs.Count].transform.GetChild(3);
            gb2 = MonoBehaviour.Instantiate(mygb, trans.position, trans.rotation) as GameObject;

            gb2.name = mygb.name;
            CreateMapParent();
            gb2.transform.SetParent(TheSet[TheObs.Count].transform);
            TheObs.Add(gb2);
            if (building)
            {
                bricks.list.Add('_' + gb2.name);
                Camera.main.UpdateCamera(TheSet[TheObs.Count - 1].transform.GetChild(0));
            }
            return gb2;
        }
        else return null;
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
		return temp;
	}

	private void ClearSet ()
	{
		TheSet.Clear ();
        TheObs.Clear();
	}

    public void ActivateObs()
    {
        foreach (var e in TheObs)
        {
            e.SetActive(true);
            e.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void CamToLastObs()
    {
        if(TheObs.Count == 0)
            Camera.main.UpdateCamera(TheSet[0].transform.GetChild(0));
        else
            Camera.main.UpdateCamera(TheSet[TheObs.Count-1].transform.GetChild(0));
    }
    public void CamToLastBrick()
    {
        Camera.main.UpdateCamera(TheSet[TheSet.Count - 1].transform.GetChild(0));
    }
    #endregion

    #region calculations

    private void DoCalculations()
    {
        Debug.Log("calculations is about to Start");
        if (Code != null) return;
        Debug.Log("calculations Started");
        CalculateCreatedDate();
        CalculateCreator();
        CalculateCount();
        CalculateBounds();
        CalculateCode();
    }

    private void CalculateCreatedDate()
    {
        Info.SetDateNow();
    }

    private void CalculateCreator()
    {
        Info.Creator = Player.Data.Creator();
    }

    private void CalculateCount()
    {
        Info.BrickCount = Bricks.List.Count;
    }

    private void CalculateCode()
    {
        Code = Player.Data.Id.Substring(20);
        Code += Info.BrickCount.ToString("X");
        Code += (Info.DateCreated.Year % 100).ToString("X");
        Code += Info.DateCreated.DayOfYear.ToString("X");
        Code += Info.DateCreated.Hour.ToString("X");
        Code += Info.DateCreated.Minute.ToString("X");
        Code += Info.DateCreated.Second.ToString("X");
        Code += Info.DateCreated.Millisecond.ToString("X").Substring(0, 1);
    }

    public void CalculateBounds()
    {
        foreach (var t in _theSet)
        {
            var trans = t.transform.position;
            if (trans.x < Info.MinBound.X)
                Info.MinBound.X = trans.x;
            if (trans.x > Info.MaxBound.X)
                Info.MaxBound.X = trans.x;

            if (trans.y < Info.MinBound.Y)
                Info.MinBound.Y = trans.y;
            if (trans.y > Info.MaxBound.Y)
                Info.MaxBound.Y = trans.y;

            if (trans.z < Info.MinBound.Z)
                Info.MinBound.Z = trans.z;
            if (trans.z > Info.MaxBound.Z)
                Info.MaxBound.Z = trans.z;
        }
        Info.Center = new Info.OurVector3((Info.MaxBound.Get() + Info.MinBound.Get()) / 2);
    }

    public string FullFileName()
    {
        return FILE.D.Maps + FileName() + FILE.Ext;
    }

    public string FileName()
    {
        return Code;
    }

    public string GetSaveable()
    {
        DoCalculations();
        return Code + "#" + Info.GetSaveable() + "#" + Bricks.GetSaveable();
    }

    public void SetSaveable(string s)
    {
        var a = s.Split('#');
        Code = a[0];
        Info.SetSaveable(a[1]);
        Bricks.SetSaveable(a[2]);
    }
}