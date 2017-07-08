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

	public static void Download (string uploadId, Map m)
	{
		new GetUploadedRequest()
		.SetUploadId(uploadId)
		.Send((response) => {
			GSData scriptData = response.ScriptData; 
			var size = response.Size; 
			string url = response.Url; 

			Auth.inst.StartCoroutine (DoDownload (response.Url,m));	
			});
	}

	private static IEnumerator DoDownload ( string uploadUrl, Map m)
	{
		WWW w = new WWW (uploadUrl);
		yield return w;
		m.bricks.SetSaveable(w.text);
		

		if (w.error != null) 
		Debug.Log (w.error);
		else {
			Debug.Log (w.text);
			Debug.Log("Done Downloading");
			m.SaveAsIs();
		}
	}

	public static void Upload (Saveable obj)
	{
		UploadCompleteMessage.Listener = GetUploadMessage;

		new GetUploadUrlRequest ().Send ((response) => {

			Auth.inst.StartCoroutine (Upload (obj,response.Url));	

			});
	}

	private static IEnumerator Upload (Saveable obj, string uploadUrl)
	{
		var form = new WWWForm ();
		form.AddBinaryData ("file", File.ReadAllBytes(obj.FullFileName()), obj.FileName());
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

	public static void AddToMapsCollection(Saveable obj){
		Auth.inst.StartCoroutine(SendWhenReady(obj));
	}

	private static IEnumerator SendWhenReady(Saveable obj){
		while(lastUploadId==null){
			yield return null;
		}
		new LogEventRequest ().SetEventKey ("MAP_ADD")
		.SetEventAttribute ("info", obj.GetSaveable())
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

	public static void AddPlay(){
		Debug.Log("Play Pressed");
		Debug.Log(Map.curr.info.code);
		new LogEventRequest ().SetEventKey ("MAP_PLAY")
		.SetEventAttribute ("code", Map.curr.info.code).Send ((res) => {
			GSData scriptData = res.ScriptData; 
			Debug.Log(scriptData);
			Debug.Log(res);
			});
	}
}
