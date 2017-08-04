using System;
using System.Collections.Generic;
using LC.MapUtls;
using LC.Online;
using LC.SaveLoad;
using UnityEngine;
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
    public static Map Curr;
    private static GameObject _mapParent;
    private readonly List<GameObject> _theSet;
    private static readonly GameObject[] Shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    private static readonly GameObject FinishLinePrefab = Resources.Load<GameObject>("Prefabs/YOUJUSTWON");

    public Map()
    {
        Code = null;
        _theSet = new List<GameObject>();
        Info = new Info();
        Bricks = new Bricks();
    }

//    protected bool Equals(Map other)
//    {
//        return Equals(Info, other.Info);
//    }

//    public override bool Equals(object obj)
//    {
//        if (ReferenceEquals(null, obj)) return false;
//        if (ReferenceEquals(this, obj)) return true;
//        return obj.GetType() == GetType() && Equals((Map) obj);
//    }

//    public override int GetHashCode()
//    {
//        return (Info != null ? Info.GetHashCode() : 0);
//    }


//    public static bool operator ==(Map a, Map b)
//    {
//        try
//        {
//            var x = a.Code.Equals(b.Code);
//            return x;
//        }
//        catch (Exception e)
//        {
//            return (object) a == null && (object) b == null;
//        }
//    }
//
//    public static bool operator !=(Map a, Map b)
//    {
//        return !(a == b);
//    }


    public static void GetReady(Action callBack)
    {
        Curr = new Map();
        Offline.GetMaps();
        Online.GetMaps();
        callBack();
    }

    public bool Delete()
    {
        return SaveLoadManager.Delete(this);
    }

    public void Upload(Action cb)
    {
        if (IsOnline)
        {
            cb();
            return;
        }
        Online.Upload(this, () =>
        {
            Info.IsOnline = true;
            SaveOffline();
            cb();
        });
    }

    //Normal saving with calculations
    public void SaveOffline()
    {
        SaveLoadManager.Save(this);
    }

    public static Map LoadFromOffline(string code)
    {
        var m = new Map {Code = code};
        m.LoadFromOffline();
        return m;
    }

    public void LoadFromOffline()
    {
        IsOffline = true;
        IsMine = Info.Creator.Equals(Player.Data.Creator());
        SetSaveable(SaveLoadManager.Load(this));
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
        var m = new Map {IsOnline = true};
        m.IsMine = m.Info.Creator.Equals(Player.Data.Creator());
        m.SetSaveable(s);
        return m;
    }

    public static Map[] GetOfflineMaps()
    {
        var maps = new List<Map>();
        foreach (var code in SaveLoadManager.AvailableMapsFiles())
        {
            if (code.Equals(FILE.Temp)) continue;
            Debug.Log("Map.FetchMapsInfoOffline.code: " + code);
            maps.Add(LoadFromOffline(code));
        }
        return maps.ToArray();
    }

    public void Build(bool building = false)
    {
        foreach (var brickName in Bricks.List)
        {
            AddBrick(brickName);
        }
        if (!building)
            AddBrick(FinishLinePrefab);
    }

    public void RemoveLastObject()
    {
        if (_theSet.Count < 1) return;
        if (_theSet.Count > 1)
            Camera.main.UpdateCamera(_theSet[_theSet.Count - 2].transform.GetChild(0));
        Object.Destroy(_theSet[_theSet.Count - 1]);
        _theSet.RemoveAt(_theSet.Count - 1);
        Bricks.List.RemoveAt(Bricks.List.Count - 1);
        SaveOffline();
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

    public static Map LoadTemp()
    {
        var m = new Map
        {
            Code = FILE.Temp
        };
        m.SetSaveable(SaveLoadManager.Load(m));
        m.Build(true);
        return m;
    }
}