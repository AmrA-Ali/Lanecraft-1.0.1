using System;
using LC.SaveLoad;
using UnityEngine;

namespace LC.MapUtls
{
    public class Info : ISaveable
    {
        public string Name, Creator;
        public OurDate DateCreated, DateUpdated;
        public int Difficulty, HighestScore, BrickCount;
        public OurVector3 MinBound, MaxBound, Center;
        public Stats Statistics;

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
                   Name + "!" +
                   Creator + "!" +
                   DateCreated.ToString(true) + "!" +
                   DateUpdated.ToString(true) + "!" +
                   Difficulty + "!" +
                   HighestScore + "!" +
                   BrickCount + "!" +
                   MinBound + "!" +
                   MaxBound + "!" +
                   Center + "!" +
                   Statistics;
        }

        public void SetSaveable(string s)
        {
            var f = s.Split('!');
            Name = f[0];
            Creator = f[1];
            DateCreated = new OurDate(f[2]);
            DateUpdated = new OurDate(f[3]);
            Difficulty = int.Parse(f[4]);
            HighestScore = int.Parse(f[5]);
            BrickCount = int.Parse(f[6]);
            MinBound = new OurVector3(f[7]);
            MaxBound = new OurVector3(f[8]);
            Center = new OurVector3(f[9]);
            Statistics = new Stats(f[10]);
        }

        public string FullFileName()
        {
            return "NO_NEED";
        }

        public string FileName()
        {
            return "NO_NEED";
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
}