using System;
using System.Collections.Generic;
using System.Linq;
using LC.Economy;
using LC.SaveLoad;
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

    public static void UpdateSlots(List<string> slots)
    {
        Available = new List<Slot>();
        foreach (var s in slots)
        {
            var slot = new Slot();
            slot.SetSaveable(s);
            Available.Add(slot);
        }
    }

    public static void UpdateSlotsFromOnline(Action cb)
    {
        GetAllSlots(results =>
        {
            UpdateSlots(results.GetStringList("slots"));
            cb();
        });
    }

    public static void Buy(int length, Action<bool, Slot> cb)
    {
        if (!EconomyManager.CanBuySlot(length) || Array.IndexOf(Lengths, length) == -1)
        {
            cb(false, null);
            return;
        }

        BuySlot(length, results =>
        {
            UpdateSlots(results.GetStringList("slots"));
            if ((bool) results.BaseData["status"])
            {
                var s = new Slot();
                s.SetSaveable((string) results.BaseData["slot"]);
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
        if (s.Map != null || m.Slot != null)
        {
            cb(false);
            return;
        }

        AddToSlot(s.Id, m.Code, results =>
        {
            UpdateSlots(results.GetStringList("slots"));
            if ((bool) results.BaseData["status"])
            {
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
        var s = Available.FirstOrDefault(slot => slot.Map == m);
        if (s.Map == null || s.Remaining <= MinThreshold)
        {
            cb(false);
            return;
        }

        RemoveFromSlot(m.Code, results =>
        {
            UpdateSlots(results.GetStringList("slots"));
            if ((bool) results.BaseData["status"])
            {
                cb(true);
                m.Slot = null;
                m.IsShared = false;
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
        if (Map == null)
            return "" + Id + "!" + Length + "!" + Remaining + "!null";
        return "" + Id + "!" + Length + "!" + Remaining + "!" + Map.Code;
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
            Empty = true;
            return;
        }
        var map = Map.GetMyMaps().Find(m => m.Code == a[3]);
        if (map != null)
        {
            Map = map;
            map.Slot = this;
            map.IsShared = true;
            Empty = false;
        }
    }
}