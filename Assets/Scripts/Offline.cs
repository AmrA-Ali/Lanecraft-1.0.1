using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;
using UnityEngine.UI;

public class Offline  {

	public static Map[] maps;
	public static bool mapsReady=false;

	public static void GetMaps(){
		mapsReady=false;
		maps = Map.FetchMapsInfoOffline ();
		mapsReady=true;
	}
}
