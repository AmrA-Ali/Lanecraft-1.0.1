using System;
using System.Collections.Generic;
using LC.SaveLoad;
using UnityEngine;

namespace LC.MapUtls
{
    public class Bricks : ISaveable
    {
        public List<string> List;
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
            return "NO_NEED";
        }

        public string FileName()
        {
            return "NO_NEED";
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
}