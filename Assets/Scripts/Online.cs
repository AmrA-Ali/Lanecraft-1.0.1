using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSparks;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Api.Messages;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Online  {

	public static Map[] maps;
	public static bool mapsReady=false;
	public static string lastUploadId=null;

	public static void Download (string uploadId, string fullFileName,Map m)
	{
		new GetUploadedRequest()
		.SetUploadId(uploadId)
		.Send((response) => {
			GSData scriptData = response.ScriptData; 
			var size = response.Size; 
			string url = response.Url; 

			Auth.inst.StartCoroutine (DoDownload (fullFileName,response.Url,m));	
			});
	}

	private static IEnumerator DoDownload (string fullFileName, string uploadUrl, Map m)
	{
		WWW w = new WWW (uploadUrl);
		yield return w;
		using(var ms  = new MemoryStream(w.bytes)){
			BinaryFormatter bf = new BinaryFormatter ();
			Bricks b = (Bricks)bf.Deserialize (ms);
			Debug.Log(b.list.Count);
			m.bricks = b;
		}

		if (w.error != null) 
		Debug.Log (w.error);
		else {
			Debug.Log (w.text);
			Debug.Log("Done Downloading");
			m.SaveAsIs();
		}
	}

	public static void Upload (string fullFileName, string name)
	{
		UploadCompleteMessage.Listener = GetUploadMessage;

		new GetUploadUrlRequest ().Send ((response) => {

			Auth.inst.StartCoroutine (Upload (fullFileName,name,response.Url));	

			});
	}

	private static IEnumerator Upload (string fullFileName, string name, string uploadUrl)
	{
		var form = new WWWForm ();
		form.AddBinaryData ("file", File.ReadAllBytes(fullFileName), name);
		WWW w2 = new WWW (uploadUrl, form);
		yield return w2;

		if (w2.error != null) 
		Debug.Log (w2.error);
		else 
		Debug.Log("Done Uploading");
		
	}
	public static void GetUploadMessage (GSMessage message)
	{
		lastUploadId = message.BaseData.GetString ("uploadId");
	}

	public static void AddToMapsCollection(string s){
		Auth.inst.StartCoroutine(SendWhenReady(s));
	}

	private static IEnumerator SendWhenReady(string s){
		while(lastUploadId==null){
			yield return null;
		}
		new LogEventRequest ().SetEventKey ("MAP_ADD")
		.SetEventAttribute ("info", s)
		.SetEventAttribute ("id", lastUploadId)
		.Send ((res) => {
			lastUploadId=null;
			});
		yield break;
	}

	public static void GetMaps(){
		mapsReady=false;
		new LogEventRequest ().SetEventKey ("MAP_GET").Send ((res) => {
			maps = Map.CollectionToMaps ((Dictionary<string,object>)res.ScriptData.BaseData);

			// maps = Array.FindAll(maps, m1 => !Array.Exists(Offline.maps, m2 => m1==m2));//Filtering out all offline maps

			mapsReady=true;
			});
	}
}
