using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LC.SaveLoad;
using UnityEngine;
using LC.Economy;

namespace LC.Online
{
    public class Slot : Saveable
    {
        public string id;
        public int duration;
        public float passed;
        public Map map;

        public static List<Slot> available;

        public static readonly int[] LENGTHS = {1, 3, 7};
        public static readonly float MIN_THRESHOLD = 0.2f;

        public Slot()
        {
            id = "00000000000000";
            duration = -1;
            passed = -1;
            map = null;
        }

        public static void Buy(int length, Action<bool> cb)
        {
            if (!EconomyManager.CanBuySlot() || Array.IndexOf(LENGTHS, length) == -1)
            {
                cb(false);
                return;
            }

            Online.BuySlot(length, results =>
            {
                if ((string) results["status"] == "true")
                {
                    var s = new Slot();
                    s.SetSaveable((string) results["slot"]);
                    available.Add(s);
                    cb(true);
                }
                else
                {
                    cb(false);
                }
            });
        }

        public static void Add(Slot s, Map m, Action<bool> cb)
        {
            if (s.map != null)
            {
                cb(false);
                return;
            }

            Online.AddToSlot(s.id, m.info.code, results =>
            {
                if ((string) results["status"] == "true")
                {
                    foreach (var slot in available)
                    {
                        if (slot.id != s.id) continue;
                        slot.map = m;
                        break;
                    }
                    cb(true);
                }
                else
                {
                    cb(false);
                }
            });
        }

        public static void Remove(Map m, Action<bool> cb)
        {
            var s = available.FirstOrDefault(slot => slot.map.info.code == m.info.code);
            if (s.map == null || s.duration - s.passed <= MIN_THRESHOLD)
            {
                cb(false);
                return;
            }

            Online.RemoveFromSlot(m.info.code, results =>
            {
                if ((string) results["status"] == "true")
                {
                    foreach (var slot in available)
                    {
                        if (slot.map.info.code != m.info.code) continue;
                        slot.map = null;
                        break;
                    }
                    cb(true);
                }
                else
                {
                    cb(false);
                }
            });
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
            return "" + id + "!" + duration + "!" + passed + "!" + map.info.code;
        }

        public void SetSaveable(string s)
        {
            var a = s.Split('!');
            id = a[0];
            duration = int.Parse(a[1]);
            passed = float.Parse(a[2]);
            foreach (var m in Offline.maps)
            {
                if (m.info.code == a[3])
                {
                    map = m;
                }
            }
        }
    }
}