using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OnlineFetchMapList : MonoBehaviour
{

	[SerializeField]
	private GameObject MapButton;
	public static string PlaySceneName = "_Play";

	void Awake ()
	{
		new LogEventRequest ().SetEventKey ("MAP_GET").Send ((res) => {
			print ("Event Recieved: MAP_GET");
			GSData scriptData = res.ScriptData; 
			Map[] ListofMaps = Map.FetchMapsInfoOnline ((Dictionary<string,object>)res.ScriptData.BaseData);
			GameObject gb;
			for (int i = 0; i < ListofMaps.Length; i++) {
				gb = Instantiate (MapButton);
				string MapName = ListofMaps [i].info.name; 
				gb.GetComponentInChildren<Text> ().text = MapName;
				gb.name = PlaySceneName;//Not needed with ModifiedMapSelectButton as it's hardcoded in the children
				foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {//The loop is needed for the ModifiedMapSelecButton
					j.selectedMap = ListofMaps [i];
				}
				gb.transform.SetParent (transform);
				gb.transform.localScale = new Vector3 (1, 1, 1);
			}
		});
	}


}