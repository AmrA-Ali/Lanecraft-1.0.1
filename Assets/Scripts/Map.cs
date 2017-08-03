using System;
using System.Collections.Generic;
using LC.SaveLoad;
using UnityEngine;
using Object = UnityEngine.Object;
using LC.Online;

public class Map : ISaveable
{
    protected bool Equals(Map other)
    {
        return Equals(Info, other.Info);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((Map) obj);
    }

    public override int GetHashCode()
    {
        return (Info != null ? Info.GetHashCode() : 0);
    }

    #region variables

    public Info Info;
    public Bricks Bricks;
    public static Map Curr;
    private static GameObject mapParent;
    public bool IsOffline;
    public bool IsOnline;
    public bool IsMine;
    public bool IsShared;
    public Slot Slot;
    private List<GameObject> _theSet;
    private static GameObject[] _shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    private static GameObject _finishLinePrefab = Resources.Load<GameObject>("Prefabs/YOUJUSTWON");

    #endregion

    #region ctor

    public Map()
    {
        _theSet = new List<GameObject>();
        Info = new Info();
        Bricks = new Bricks();
    }

    public static bool operator ==(Map a, Map b)
    {
        try
        {
            var x = a.Info.Code.Equals(b.Info.Code);
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

    #endregion

    public void UpdateInfo()
    {
        SaveLoadManager.Save(Info);
    }

    public bool Delete()
    {
        return SaveLoadManager.Delete(Info) && SaveLoadManager.Delete(Bricks);
    }

    #region online

    public void Upload(Action cb)
    {
        if (IsOnline)
        {
            cb();
            return;
        }
        Debug.Log("MAP: Uploading...");
        Info.IsOnline = true;
        Bricks.Code = Info.Code;
        FetchBricks();
        Online.Upload(this, () =>
        {
            UpdateInfo();
            cb();
        });
    }

    //Download the bricks
    public void Download()
    {
        //Don't download, the bricks is already here
        //Online.Download(this.uploadId,this);	
    }

    #endregion

    #region loading

    //Save Map without doing Calculations
    public void SaveAsIs()
    {
        Debug.Log("Map: " + "SaveAsIs");
        Bricks.Code = Info.Code;
        SaveLoadManager.Save(Bricks);
        SaveLoadManager.Save(Info);
    }

    //Normal saving with calculations
    public void Save()
    {
        Debug.Log("Map: " + "Save");
        DoCalculations();
        SaveLoadManager.Save(Bricks);
        SaveLoadManager.Save(Info);
    }

    public GameObject MapParent()
    {
        try
        {
            var unused = mapParent.activeInHierarchy; //Any test metod to see if it's there
        }
        catch (Exception)
        {
            mapParent = new GameObject {name = "Track"};
        }
        return mapParent;
    }

    public void FetchBricks()
    {
        if (Bricks.ready) return;
        Bricks.Code = Info.Code;
        Bricks.SetSaveable(SaveLoadManager.Load(Bricks));
    }

    public static Map CollectionToMap(Dictionary<string, object> dict)
    {
        var info = (string) dict["info"];
        var bricks = (string) dict["bricks"];
        var m = new Map();
        m.Info.SetSaveable(info);
        m.Bricks.SetSaveable(bricks);
        m.IsOffline = false;
        m.IsMine = m.Info.Creator.Equals(Player.Data.Creator());
        return m;
    }

    public static Map[] FetchMapsInfoOffline()
    {
        var maps = new List<Map>();
        foreach (var entry in SaveLoadManager.FetchMapsInfoCodes())
        {
            var code = entry.FilterFileExtension(FILE.Ext);
            if (code.Equals(FILE.Temp)) continue;
            Debug.Log("Map.FetchMapsInfoOffline.code: " + code);
            var m = new Map {Info = {Code = code}};
            m.Info.SetSaveable(SaveLoadManager.Load(m.Info));
            m.IsOffline = true;
            m.IsMine = m.Info.Creator.Equals(Player.Data.Creator());
            maps.Add(m);
        }
        return maps.ToArray();
    }

    #endregion

    #region brickbuilding

    public void Build(bool building = false)
    {
        FetchBricks();
        foreach (var brickName in Bricks.List)
        {
            AddBrick(brickName);
        }
        if (!building)
            AddBrick(_finishLinePrefab);
    }

    public void RemoveLastObject()
    {
        if (_theSet.Count < 1) return;
        if (_theSet.Count > 1)
            Camera.main.UpdateCamera(_theSet[_theSet.Count - 2].transform.GetChild(0));
        Object.Destroy(_theSet[_theSet.Count - 1]);
        _theSet.RemoveAt(_theSet.Count - 1);
        Bricks.List.RemoveAt(Bricks.List.Count - 1);
        SaveAsIs();
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
        foreach (var e in _shapes)
        {
            if (objectName != e.name) continue;
            AddBrick(e, building);
            if (building) SaveAsIs();
            break;
        }
    }

    #endregion

    #region calculations

    public void DoCalculations()
    {
        CalculateCreatedDate();
        CalculateCreator();
        CalculateCount();
        CalculateBounds();
        CalculateCode();
    }

    void CalculateCreatedDate()
    {
        Info.SetDateNow();
    }

    void CalculateCreator()
    {
        Info.Creator = Player.Data.Creator();
    }

    void CalculateCount()
    {
        Info.BrickCount = Bricks.List.Count;
    }

    private void CalculateCode()
    {
        Info.Code = Player.Data.Id.Substring(20);
        Info.Code += Info.BrickCount.ToString("X");
        Info.Code += (Info.DateCreated.Year % 100).ToString("X");

        Info.Code += Info.DateCreated.DayOfYear.ToString("X");
        Info.Code += Info.DateCreated.Hour.ToString("X");
        Info.Code += Info.DateCreated.Minute.ToString("X");
        Info.Code += Info.DateCreated.Second.ToString("X");
        Info.Code += Info.DateCreated.Millisecond.ToString("X").Substring(0, 1);

        Bricks.Code = Info.Code;
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

    #endregion

    public string FullFileName()
    {
        return Info.FullFileName();
    }

    public string FileName()
    {
        return Info.FileName();
    }

    public string GetSaveable()
    {
        return Info.GetSaveable() + "#" + Bricks.GetSaveable();
    }

    public void SetSaveable(string s)
    {
        var a = s.Split('#');
        Info.SetSaveable(a[0]);
        Bricks.SetSaveable(a[1]);
    }

    public static Map LoadTemp()
    {
        Debug.Log("Map: " + "LoadTemp");
        var m = new Map
        {
            Info = {Code = FILE.Temp},
            Bricks = {Code = FILE.Temp}
        };
        m.Info.SetSaveable(SaveLoadManager.Load(m.Info));
        m.Bricks.SetSaveable(SaveLoadManager.Load(m.Bricks));
        m.Build(true);
        return m;
    }

    public static void GetReady(Action callBack)
    {
        Curr = new Map();
        Offline.GetMaps();
        Online.GetMaps();
        callBack();
    }
}

public class Bricks : ISaveable
{
    public List<string> List;
    public string Code;
    public bool ready;

    public Bricks()
    {
        List = new List<string>();
    }

    public Bricks(List<GameObject> theSet)
    {
        foreach (var t in theSet)
        {
            List.Add(t.name.Substring(0, t.name.Length - "(Clone)".Length));
        }
    }

    public Bricks(int[] a)
    {
        List = new List<string>(ToString(a));
    }

    public string FullFileName()
    {
        return FILE.D.Bricks + FileName() + FILE.Ext;
    }

    public string FileName()
    {
        return Code;
    }

    public string GetSaveable()
    {
        return List.Count == 0 ? "-1" : string.Join("!", Array.ConvertAll(ToInt(), i => i.ToString()));
    }

    public void SetSaveable(string s)
    {
        var a = Array.ConvertAll(s.Split('!'), int.Parse);
        List = a[0] == -1 ? new List<string>() : new List<string>(ToString(a));
        ready = true;
    }

    public int[] ToInt()
    {
        return List.ConvertAll(StringToInt).ToArray();
    }

    public string[] ToString(int[] a)
    {
        return Array.ConvertAll(a, IntToString);
    }

    private static int StringToInt(string s)
    {
        var theDict = new Dictionary<string, int>
        {
            {"Line", 1},
            {"TurnRight", 2},
            {"TurnLeft", 3},
            {"CurveUp", 4},
            {"CurveDown", 5},
            {"TightRight", 6}
        };
        return theDict[s];
    }

    private static string IntToString(int i)
    {
        var theDict = new Dictionary<int, string>
        {
            {1, "Line"},
            {2, "TurnRight"},
            {3, "TurnLeft"},
            {4, "CurveUp"},
            {5, "CurveDown"},
            {6, "TightRight"}
        };
        return theDict[i];
    }
}

public class Info : ISaveable
{
    public string Code, Name, Creator;
    public OurDate DateCreated, DateUpdated;
    public int Difficulty, HighestScore;
    public bool IsOnline;
    public int BrickCount;
    public OurVector3 MinBound, MaxBound, Center;
    public Stats Statistics;

    protected bool Equals(Info other)
    {
        return Equals(Code, other.Code);
    }

    public Info()
    {
        Name = "TEMP";
        Creator = "You";
        MinBound = new OurVector3();
        MaxBound = new OurVector3();
        Center = new OurVector3();
        Statistics = new Stats();
        DateCreated = new OurDate(DateTime.Now);
        DateUpdated = new OurDate(DateTime.Now);
    }

    public string GetSaveable()
    {
        return "" +
               Code + "!" +
               Name + "!" +
               Creator + "!" +
               DateCreated.ToString(true) + "!" +
               DateUpdated.ToString(true) + "!" +
               Difficulty + "!" +
               HighestScore + "!" +
               IsOnline + "!" +
               BrickCount + "!" +
               MinBound + "!" +
               MaxBound + "!" +
               Center + "!" +
               Statistics;
    }

    public void SetSaveable(string s)
    {
        var f = s.Split('!');
        Code = f[0];
        Debug.Log("Map.Info.SetSavable.code: " + Code);
        Name = f[1];
        Creator = f[2];
        DateCreated = new OurDate(f[3]);
        DateUpdated = new OurDate(f[4]);
        Difficulty = int.Parse(f[5]);
        HighestScore = int.Parse(f[6]);
        IsOnline = bool.Parse(f[7]);
        BrickCount = int.Parse(f[8]);
        MinBound = new OurVector3(f[9]);
        MaxBound = new OurVector3(f[10]);
        Center = new OurVector3(f[11]);
        Statistics = new Stats(f[12]);
    }

    public string FullFileName()
    {
        return FILE.D.Info + FileName() + FILE.Ext;
    }

    public string FileName()
    {
        return Code;
    }

    public void SetDateNow()
    {
        DateCreated = new OurDate(DateTime.Now);
        DateUpdated = DateCreated;
    }

    public class OurDate
    {
        public int Year, DayOfYear, Hour, Minute, Second, Millisecond;

        public OurDate(DateTime t)
        {
            Year = t.Year;
            DayOfYear = t.DayOfYear;
            Hour = t.Hour;
            Minute = t.Minute;
            Second = t.Second;
            Millisecond = t.Millisecond;
        }

        public OurDate(string t)
        {
            int[] f = Array.ConvertAll(t.Split(':'), s => int.Parse(s));
            Year = f[0];
            DayOfYear = f[1];
            Hour = f[2];
            Minute = f[3];
            Second = f[4];
            Millisecond = f[5];
        }

        public override string ToString()
        {
            return "" + DayOfYear + "/" + Year + " - " + Hour + ":" + Minute;
        }

        public string ToString(bool f)
        {
            return "" + Year + ":" + DayOfYear + ":" + Hour + ":" + Minute + ":" + Second + ":" + Millisecond;
        }
    }

    public class Stats
    {
        public int TurnRights, TurnLefts, CurveUps, CurveDowns, Lines;
        public int ObstacleCount;

        public Stats()
        {
        }

        public Stats(string s)
        {
            int[] f = Array.ConvertAll(s.Split(':'), x => int.Parse(x));
            TurnRights = f[0];
            TurnLefts = f[1];
            CurveUps = f[2];
            CurveDowns = f[3];
            Lines = f[4];
            ObstacleCount = f[5];
        }

        public override string ToString()
        {
            return "" + TurnRights + ":" + TurnLefts + ":" + CurveUps + ":" + CurveDowns + ":" + Lines + ":" +
                   ObstacleCount;
        }
    }

    public class OurVector3
    {
        public float X, Y, Z;

        public OurVector3()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public OurVector3(string s)
        {
            var f = Array.ConvertAll(s.Split(':'), float.Parse);
            X = f[0];
            Y = f[1];
            Z = f[2];
        }

        public OurVector3(Vector3 vec)
        {
            X = vec.x;
            Y = vec.y;
            Z = vec.z;
        }

        public Vector3 Get()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return "" + X + ":" + Y + ":" + Z;
        }
    }
}