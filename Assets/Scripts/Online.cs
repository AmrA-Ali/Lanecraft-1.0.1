using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GameSparks.Api.Requests;
using GameSparks.Core;
using LC.SaveLoad;
using UnityEngine;

namespace LC.Online
{
    public class Online
    {
        public static Map[] Maps;
        public static bool MapsReady;

        public static void RateMap(string code, int rating, Action cb)
        {
            new LogEventRequest().SetEventKey("MAP_RATING")
                .SetEventAttribute("code", code)
                .SetEventAttribute("rating", rating).Send(
                    res =>
                    {
                        Debug.Log("Map Rating: " + ((Dictionary<string, object>) res.ScriptData.BaseData)["status"]);
                        cb();
                    });
        }

        public static void FavoriteMap(string code, Action cb)
        {
            new LogEventRequest().SetEventKey("MAP_FAVORITE")
                .SetEventAttribute("code", code).Send(
                    res =>
                    {
                        Debug.Log("Map Favorite: " + ((Dictionary<string, object>) res.ScriptData.BaseData)["status"]);
                        cb();
                    });
        }


        public static void Upload(ISaveable obj, Action cb)
        {
            new LogEventRequest().SetEventKey("MAP_ADD")
                .SetEventAttribute("map", obj.GetSaveable())
                .Send(res =>
                {
                    Debug.Log("Map Upload: " + ((Dictionary<string, object>) res.ScriptData.BaseData)["status"]);
                    cb();
                });
        }

        public static void GetMaps(Action callBack)
        {
            if (!Player.Online || !Player.Authenticated)
            {
                callBack();
                return;
            }

            MapsReady = false;
            new LogEventRequest().SetEventKey("MAP_GET_ALL").Send(res =>
            {
                var mapsList = new List<Map>();

                var l = (List<object>) res.GetDict()["maps"];
                foreach (var o in l)
                {
                    var d = (Dictionary<string, object>) o;
                    mapsList.Add(Map.LoadFromOnline((string) d["map"]));
                }
                Maps = mapsList.ToArray();
                // maps = Array.FindAll(maps, m1 => !Array.Exists(Offline.maps, m2 => m1==m2));//Filtering out all offline maps
                MapsReady = true;
                callBack();
            });
        }

        public static void Qp(Action<Dictionary<string, object>> cb)
        {
            Debug.Log("QP Started...");
            new LogEventRequest().SetEventKey("MAP_GET_QUICK_PLAY").Send(res =>
            {
                if (!(bool) res.ScriptData.BaseData["status"])
                {
                    Debug.Log("Online.QuickPlay: False");
                    cb(null);
                }
                else
                {
                    var map = (Dictionary<string, object>) ((GSData) res.GetDict()["map"]).BaseData;
                    cb(map);
                }
            });
        }

        public static string GetHtmlFromUri(string resource)
        {
            var html = string.Empty;
            var req = (HttpWebRequest) WebRequest.Create(resource);
            try
            {
                using (var resp = (HttpWebResponse) req.GetResponse())
                {
                    var isSuccess = (int) resp.StatusCode < 299 && (int) resp.StatusCode >= 200;
                    if (isSuccess)
                    {
                        using (var reader = new StreamReader(resp.GetResponseStream()))
                        {
                            //We are limiting the array to 80 so we don't have
                            //to parse the entire html document feel free to 
                            //adjust (probably stay under 300)
                            var cs = new char[80];
                            reader.Read(cs, 0, cs.Length);
                            foreach (var ch in cs)
                            {
                                html += ch;
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            return html;
        }

        public static bool IsConnectedToInternet()
        {
            var htmlText = GetHtmlFromUri("http://google.com");
            if (htmlText == "")
            {
                //No connection
                return false;
            }
            if (!htmlText.Contains("schema.org/WebPage"))
            {
                //Redirecting since the beginning of googles html contains that 
                //phrase and it was not found
                return false;
            }
            //success
            return true;
        }
    }

    public class Slot
    {
        protected static void BuySlot(int length, Action<GSData> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_BUY")
                .SetEventAttribute("length", length)
                .Send(res =>
                {
                    var results = res.ScriptData;
                    Debug.Log("Online.BuySlot: " + results.BaseData["status"]);
                    cb(results);
                });
        }

        protected static void AddToSlot(string slotId, string mapId, Action<GSData> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_ADD")
                .SetEventAttribute("slotId", slotId)
                .SetEventAttribute("mapId", mapId)
                .Send(res =>
                {
                    var results = res.ScriptData;
                    Debug.Log("Online.AddToSlot: " + results.BaseData["status"]);
                    cb(results);
                });
        }

        protected static void RemoveFromSlot(string mapId, Action<GSData> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_REMOVE")
                .SetEventAttribute("mapId", mapId)
                .Send(res =>
                {
                    var results = res.ScriptData;
                    Debug.Log("Online.RemoveFromSlot: " + results.BaseData["status"]);
                    cb(results);
                });
        }

        protected static void GetAllSlots(Action<GSData> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_GET_ALL")
                .Send(res =>
                {
                    var results = res.ScriptData;
                    Debug.Log("Online.GetAllSlots: " + results.BaseData["status"]);
                    cb(results);
                });
        }
    }
}