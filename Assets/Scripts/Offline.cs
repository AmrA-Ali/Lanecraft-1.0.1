using System;
using System.Collections.Generic;
using UnityEngine;

public class Offline
{
    public static Map[] Maps;
    public static bool MapsReady;


    public static void GetMaps()
    {
        MapsReady = false;
        Maps = Map.GetOfflineMaps();
        MapsReady = true;
    }
}