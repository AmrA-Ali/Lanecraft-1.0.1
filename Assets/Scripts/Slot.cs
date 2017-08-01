using System;
using System.Collections.Generic;
using System.Linq;
using LC.SaveLoad;
using LC.Economy;
using UnityEngine;

public class Slot : LC.Online.Slot, Saveable
{
    public string id;
    public int length;
    public int remaining;
    public Map map;

    public static List<Slot> available;

    public static readonly int[] LENGTHS = {1, 3, 7};
    public static readonly int MIN_THRESHOLD = 2;

    public Slot()
    {
        id = "00000000000000";
        length = -1;
        remaining = -1;
        map = null;
    }

    public static void Buy(int length, Action<bool> cb)
    {
        if (!EconomyManager.CanBuySlot() || Array.IndexOf(LENGTHS, length) == -1)
        {
            cb(false);
            return;
        }

        BuySlot(length, results =>
        {
            if ((bool) results.BaseData["status"])
            {
                var s = new Slot();
                s.SetSaveable((string) results.BaseData["slot"]);
                if (available == null)
                    available = new List<Slot>();

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
        Debug.Log(s.map);
        if (s.map != null)
        {
            cb(false);
            return;
        }
        Debug.Log("Slot.Add: Passed Validations.");

        AddToSlot(s.id, m.info.code, results =>
        {
            if ((bool) results["status"])
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
        if (s.map == null || s.remaining <= MIN_THRESHOLD)
        {
            cb(false);
            return;
        }

        RemoveFromSlot(m.info.code, results =>
        {
            if ((bool) results["status"])
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
        return "" + id + "!" + length + "!" + remaining + "!" + map.info.code;
    }

    public void SetSaveable(string s)
    {
        var a = s.Split('!');
        id = a[0];
        length = int.Parse(a[1]);
        remaining = int.Parse(a[2]);
        if (a[3] == "null")
        {
            map = null;
            return;
        }
        foreach (var m in Offline.maps)
        {
            if (m.info.code == a[3])
            {
                map = m;
            }
        }
    }
}