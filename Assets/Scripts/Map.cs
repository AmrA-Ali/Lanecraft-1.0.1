using System;
using System.Collections.Generic;
using LC.SaveLoad;
using UnityEngine;
using Object = UnityEngine.Object;

public class Map : Saveable
{
    protected bool Equals(Map other)
    {
        return Equals(info, other.info);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((Map) obj);
    }

    public override int GetHashCode()
    {
        return (info != null ? info.GetHashCode() : 0);
    }

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
    private static GameObject[] Shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");
    private static GameObject FinishLinePrefab = Resources.Load<GameObject>("Prefabs/YOUJUSTWON");

    #endregion

    #region ctor

    public Map()
    {
        TheSet = new List<GameObject>();
        info = new Info();
        bricks = new Bricks();
    }

    public static bool operator ==(Map a, Map b)
    {
        return a.info.code.Equals(b.info.code);
    }

    public static bool operator !=(Map a, Map b)
    {
        return !(a == b);
    }

    #endregion

    public void UpdateInfo()
    {
        SaveLoadManager.Save(info);
    }

    public bool Delete()
    {
        return SaveLoadManager.Delete(info) && SaveLoadManager.Delete(bricks);
    }

    #region online

    public void Upload()
    {
        Debug.Log("MAP: Uploading...");
        info.isOnline = true;
        bricks.code = info.code;
        FetchBricks();
        Online.Upload(this);
        UpdateInfo();
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
        bricks.code = info.code;
        SaveLoadManager.Save(bricks);
        SaveLoadManager.Save(info);
    }

    //Normal saving with calculations
    public void Save()
    {
        Debug.Log("Map: " + "Save");
        DoCalculations();
        SaveLoadManager.Save(bricks);
        SaveLoadManager.Save(info);
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
        bricks.code = info.code;
        bricks.SetSaveable(SaveLoadManager.Load(bricks));
    }

    public static Map CollectionToMap(Dictionary<string, object> dict)
    {
        var info = (string) dict["info"];
        var bricks = (string) dict["bricks"];
        var m = new Map();
        m.info.SetSaveable(info);
        m.bricks.SetSaveable(bricks);
        m.isOffline = false;
        m.isMine = m.info.creator.Equals(Player.DATA.Creator());
        return m;
    }

    public static Map[] FetchMapsInfoOffline()
    {
        var maps = new List<Map>();
        foreach (var entry in SaveLoadManager.FetchMapsInfoCodes())
        {
            var code = entry.FilterFileExtension(FILE.EXT);
            if (code.Equals(FILE.TEMP)) continue;
            Debug.Log("Map.FetchMapsInfoOffline.code: " + code);
            var m = new Map {info = {code = code}};
            m.info.SetSaveable(SaveLoadManager.Load(m.info));
            m.isOffline = true;
            m.isMine = m.info.creator.Equals(Player.DATA.Creator());
            maps.Add(m);
        }
        return maps.ToArray();
    }

    #endregion

    #region brickbuilding

    public void Build(bool building = false)
    {
        FetchBricks();
        foreach (var brickName in bricks.list)
        {
            AddBrick(brickName);
        }
        if (!building)
            AddBrick(FinishLinePrefab);
    }

    public void RemoveLastObject()
    {
        if (TheSet.Count < 1) return;
        if (TheSet.Count > 1)
            Camera.main.UpdateCamera(TheSet[TheSet.Count - 2].transform.GetChild(0));
        Object.Destroy(TheSet[TheSet.Count - 1]);
        TheSet.RemoveAt(TheSet.Count - 1);
        bricks.list.RemoveAt(bricks.list.Count - 1);
        SaveAsIs();
    }

    private void AddBrick(GameObject mygb, bool building = false)
    {
        GameObject gb2;
        if (TheSet.Count == 0)
        {
            gb2 = Object.Instantiate(mygb);
            gb2.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            var trans = TheSet[TheSet.Count - 1].transform.GetChild(1);
            gb2 = Object.Instantiate(mygb, trans.position, trans.rotation);
        }
        gb2.name = mygb.name;
        gb2.transform.SetParent(MapParent().transform);
        TheSet.Add(gb2);
        if (!building) return;
        bricks.list.Add(gb2.name);
        Camera.main.UpdateCamera(gb2.transform.GetChild(0));
    }

    public void AddBrick(string objectName, bool building = false)
    {
        foreach (var e in Shapes)
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
        info.SetDateNow();
    }

    void CalculateCreator()
    {
        info.creator = Player.DATA.Creator();
    }

    void CalculateCount()
    {
        info.brickCount = bricks.list.Count;
    }

    private void CalculateCode()
    {
        info.code = Player.DATA.id.Substring(20);
        info.code += info.brickCount.ToString("X");
        info.code += (info.dateCreated.Year % 100).ToString("X");

        info.code += info.dateCreated.DayOfYear.ToString("X");
        info.code += info.dateCreated.Hour.ToString("X");
        info.code += info.dateCreated.Minute.ToString("X");
        info.code += info.dateCreated.Second.ToString("X");
        info.code += info.dateCreated.Millisecond.ToString("X").Substring(0, 1);

        bricks.code = info.code;
    }

    public void CalculateBounds()
    {
        foreach (var t in TheSet)
        {
            var trans = t.transform.position;
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
        info.center = new Info.OurVector3((info.maxBound.get() + info.minBound.get()) / 2);
    }

    #endregion

    public string FullFileName()
    {
        return info.FullFileName();
    }

    public string FileName()
    {
        return info.FileName();
    }

    public string GetSaveable()
    {
        return info.GetSaveable() + "#" + bricks.GetSaveable();
    }

    public void SetSaveable(string s)
    {
        var a = s.Split('#');
        info.SetSaveable(a[0]);
        bricks.SetSaveable(a[1]);
    }

    public static Map LoadTemp()
    {
        Debug.Log("Map: " + "LoadTemp");
        var m = new Map
        {
            info = {code = FILE.TEMP},
            bricks = {code = FILE.TEMP}
        };
        m.info.SetSaveable(SaveLoadManager.Load(m.info));
        m.bricks.SetSaveable(SaveLoadManager.Load(m.bricks));
        m.Build(true);
        return m;
    }
}

public class Bricks : Saveable
{
    public List<string> list;
    public string code;

    public Bricks()
    {
        list = new List<string>();
    }

    public Bricks(List<GameObject> TheSet)
    {
        foreach (var t in TheSet)
        {
            list.Add(t.name.Substring(0, t.name.Length - "(Clone)".Length));
        }
    }

    public Bricks(int[] a)
    {
        list = new List<string>(ToString(a));
    }

    public string FullFileName()
    {
        return FILE.D.BRICKS + FileName() + FILE.EXT;
    }

    public string FileName()
    {
        return code;
    }

    public string GetSaveable()
    {
        return list.Count == 0 ? "-1" : string.Join("!", Array.ConvertAll(ToInt(), i => i.ToString()));
    }

    public void SetSaveable(string s)
    {
        var a = Array.ConvertAll(s.Split('!'), int.Parse);
        list = a[0] == -1 ? new List<string>() : new List<string>(ToString(a));
    }

    public int[] ToInt()
    {
        return list.ConvertAll(StringToInt).ToArray();
    }

    public string[] ToString(int[] a)
    {
        return Array.ConvertAll(a, IntToString);
    }

    private static int StringToInt(string s)
    {
        var THE_DICT = new Dictionary<string, int>
        {
            {"Line", 1},
            {"TurnRight", 2},
            {"TurnLeft", 3},
            {"CurveUp", 4},
            {"CurveDown", 5},
            {"TightRight", 6}
        };
        return THE_DICT[s];
    }

    private static string IntToString(int i)
    {
        var THE_DICT = new Dictionary<int, string>
        {
            {1, "Line"},
            {2, "TurnRight"},
            {3, "TurnLeft"},
            {4, "CurveUp"},
            {5, "CurveDown"},
            {6, "TightRight"}
        };
        return THE_DICT[i];
    }
}

public class Info : Saveable
{
    public string code, name, creator;
    public OurDate dateCreated, dateUpdated;
    public int difficulty, highestScore;
    public bool isOnline;
    public int brickCount;
    public OurVector3 minBound, maxBound, center;
    public Stats statistics;

    protected bool Equals(Info other)
    {
        return Equals(code, other.code);
    }

    public Info()
    {
        name = "TEMP";
        creator = "You";
        minBound = new OurVector3();
        maxBound = new OurVector3();
        center = new OurVector3();
        statistics = new Stats();
        dateCreated = new OurDate(DateTime.Now);
        dateUpdated = new OurDate(DateTime.Now);
    }

    public string GetSaveable()
    {
        return "" +
               code + "!" +
               name + "!" +
               creator + "!" +
               dateCreated.ToString(true) + "!" +
               dateUpdated.ToString(true) + "!" +
               difficulty + "!" +
               highestScore + "!" +
               isOnline + "!" +
               brickCount + "!" +
               minBound + "!" +
               maxBound + "!" +
               center + "!" +
               statistics;
    }

    public void SetSaveable(string s)
    {
        var f = s.Split('!');
        code = f[0];
        Debug.Log("Map.Info.SetSavable.code: " + code);
        name = f[1];
        creator = f[2];
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

    public string FullFileName()
    {
        return FILE.D.INFO + FileName() + FILE.EXT;
    }

    public string FileName()
    {
        return code;
    }

    public void SetDateNow()
    {
        dateCreated = new OurDate(DateTime.Now);
        dateUpdated = dateCreated;
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
        public int turnRights, turnLefts, curveUps, curveDowns, lines;
        public int obstacleCount;

        public Stats()
        {
        }

        public Stats(string s)
        {
            int[] f = Array.ConvertAll(s.Split(':'), x => int.Parse(x));
            turnRights = f[0];
            turnLefts = f[1];
            curveUps = f[2];
            curveDowns = f[3];
            lines = f[4];
            obstacleCount = f[5];
        }

        public override string ToString()
        {
            return "" + turnRights + ":" + turnLefts + ":" + curveUps + ":" + curveDowns + ":" + lines + ":" +
                   obstacleCount;
        }
    }

    public class OurVector3
    {
        public float x, y, z;

        public OurVector3()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
        }

        public OurVector3(string s)
        {
            var f = Array.ConvertAll(s.Split(':'), float.Parse);
            x = f[0];
            y = f[1];
            z = f[2];
        }

        public OurVector3(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 get()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return "" + x + ":" + y + ":" + z;
        }
    }
}