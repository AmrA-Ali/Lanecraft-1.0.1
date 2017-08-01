using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;
using LC.SaveLoad;

namespace LC.Online
{
    public class Online
    {
        public static Map[] maps;

        public static bool mapsReady;

        public static void RateMap(string code, int rating)
        {
            new LogEventRequest().SetEventKey("MAP_RATING").SetEventAttribute("code", code)
                .SetEventAttribute("rating", rating).Send(
                    res =>
                    {
                        Debug.Log("Map Upload: " + ((Dictionary<string, object>) res.ScriptData.BaseData)["status"]);
                    });
        }

       

        public static void Upload(Saveable obj)
        {
            new LogEventRequest().SetEventKey("MAP_ADD")
                .SetEventAttribute("map", obj.GetSaveable())
                .Send((res) =>
                {
                    Debug.Log("Map Upload: " + ((Dictionary<string, object>) res.ScriptData.BaseData)["status"]);
                });
        }

        public static void GetMaps()
        {
            Debug.Log("Online.GetMaps: " + Player.AUTHENTICATED);
            if (!Player.AUTHENTICATED)
            {
                return;
            }

            mapsReady = false;
            new LogEventRequest().SetEventKey("MAP_GET").Send((res) =>
            {
                var mapsList = new List<Map>();

                var l = (List<object>) res.GetDict()["maps"];
                foreach (var o in l)
                {
                    var d = (Dictionary<string, object>) o;
                    mapsList.Add(Map.CollectionToMap(d));
                }
                maps = mapsList.ToArray();
                // maps = Array.FindAll(maps, m1 => !Array.Exists(Offline.maps, m2 => m1==m2));//Filtering out all offline maps
                mapsReady = true;
            });
        }

        public static void AddPlay()
        {
//        Debug.Log("Play Pressed");
//        Debug.Log(Map.curr.info.code);
//        new LogEventRequest().SetEventKey("MAP_PLAY")
//            .SetEventAttribute("code", Map.curr.info.code).Send((res) =>
//            {
//                GSData scriptData = res.ScriptData;
//                Debug.Log(scriptData);
//                Debug.Log(res);
//            });
        }

        public static void QP(Action<Dictionary<string, object>> cb)
        {
            Debug.Log("QP Started...");
            new LogEventRequest().SetEventKey("QUICK_PLAY").Send((res) =>
            {
                var map = (Dictionary<string, object>) ((GSData) res.GetDict()["map"]).BaseData;
                cb(map);
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
            var HtmlText = GetHtmlFromUri("http://google.com");
            if (HtmlText == "")
            {
                //No connection
                return false;
            }
            else if (!HtmlText.Contains("schema.org/WebPage"))
            {
                //Redirecting since the beginning of googles html contains that 
                //phrase and it was not found
                return false;
            }
            else
            {
                //success
                return true;
            }
        }
    }

    public class Slot
    {
        protected static void BuySlot(int length, Action<GSData> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_BUY")
                .SetEventAttribute("length", length)
                .Send((res) =>
                {
                    var results = res.ScriptData;
                    Debug.Log("Online.BuySlot: " + results.BaseData["status"]);
                    cb(results);
                });
        }

        protected static void AddToSlot(string slotId, string mapId, Action<Dictionary<string, object>> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_ADD")
                .SetEventAttribute("slotId", slotId)
                .SetEventAttribute("mapId", mapId)
                .Send((res) =>
                {
                    var results = (Dictionary<string, object>) res.ScriptData.BaseData;
                    Debug.Log("Online.AddToSlot: " + results["status"]);
                    cb(results);
                });
        }

        protected static void RemoveFromSlot(string mapId, Action<Dictionary<string, object>> cb)
        {
            new LogEventRequest().SetEventKey("SLOT_REMOVE")
                .SetEventAttribute("mapId", mapId)
                .Send((res) =>
                {
                    var results = (Dictionary<string, object>) res.ScriptData.BaseData;
                    Debug.Log("Online.RemoveFromSlot: " + results["status"]);
                    cb(results);
                });
        }
    }
}