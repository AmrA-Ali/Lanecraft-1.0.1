using System;
using System.Collections.Generic;
using System.Linq;
using LC.SaveLoad;
using LC.Economy;
using UnityEngine;

public class Slot : LC.Online.Slot, ISaveable
{
    public string Id;
    public int Length;
    public int Remaining;
    public Map Map;
    public bool Empty;
    public static List<Slot> Available;

    public static readonly int[] Lengths = {1, 3, 7};
    public static readonly int MinThreshold = 2;

    public Slot()
    {
        Id = "00000000000000";
        Length = -1;
        Remaining = -1;
        Map = null;
        Empty = true;
    }

    public static List<Slot> GetEmpty()
    {
        return Available.FindAll(x => x.Empty);
    }

    public static void UpdateFomOnline(Action cb)
    {
        GetAllSlots(results =>
        {
            var slots = results.GetStringList("slots");
            Available = new List<Slot>();
            foreach (var s in slots)
            {
                var slot = new Slot();
                slot.SetSaveable(s.ToString());
                Available.Add(slot);
            }
            cb();
        });
    }

    public static void Buy(int length, Action<bool, Slot> cb)
    {
        if (!EconomyManager.CanBuySlot() || Array.IndexOf(Lengths, length) == -1)
        {
            cb(false, null);
            return;
        }

        BuySlot(length, results =>
        {
            if ((bool) results.BaseData["status"])
            {
                var s = new Slot();
                s.SetSaveable((string) results.BaseData["slot"]);
                if (Available == null)
                    Available = new List<Slot>();

                Available.Add(s);
                cb(true, s);
            }
            else
            {
                cb(false, null);
            }
        });
    }

    public static void Add(Slot s, Map m, Action<bool> cb)
    {
        if (s.Map != null)
        {
            cb(false);
            return;
        }

        AddToSlot(s.Id, m.Info.Code, results =>
        {
            if ((bool) results.BaseData["status"])
            {
                foreach (var slot in Available)
                {
                    if (slot.Id != s.Id) continue;
                    slot.Map = m;
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
        var s = Available.FirstOrDefault(slot => slot.Map.Info.Code == m.Info.Code);
        if (s.Map == null || s.Remaining <= MinThreshold)
        {
            cb(false);
            return;
        }

        RemoveFromSlot(m.Info.Code, results =>
        {
            if ((bool) results.BaseData["status"])
            {
                foreach (var slot in Available)
                {
                    if (slot.Map.Info.Code != m.Info.Code) continue;
                    slot.Map = null;
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
        if(Map == null)
            return "" + Id + "!" + Length + "!" + Remaining + "!null";
        return "" + Id + "!" + Length + "!" + Remaining + "!" + Map.Info.Code;
    }

    public void SetSaveable(string s)
    {
        var a = s.Split('!');
        Id = a[0];
        Length = int.Parse(a[1]);
        Remaining = int.Parse(a[2]);
        if (a[3] == "null")
        {
            Map = null;
            return;
        }
        foreach (var m in Offline.Maps)
        {
            Debug.Log(m.Info.Code);
            if (m.Info.Code == a[3])
            {
                Map = m;
                Empty = false;
            }
        }
        Debug.Log(Map);
    }
}    