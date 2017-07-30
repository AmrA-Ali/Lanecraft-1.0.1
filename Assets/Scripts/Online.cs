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
			Ref.INST.StartCoroutine (DoDownload (response.Url,m));	
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
			Ref.INST.StartCoroutine (Upload (obj,response.Url));	
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
		Ref.INST.StartCoroutine(SendWhenReady(obj));
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
		Debug.Log("Online.GetMaps:" + Player.AUTHENTICATED);
		if (!Player.AUTHENTICATED){
			return;
		}

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
	public static void QP(Action cb){
		Debug.Log("QP Started...");
		new LogEventRequest ().SetEventKey ("QUICK_PLAY").Send ((res) => {
			GSData scriptData = res.ScriptData; 
			Debug.Log(scriptData);
			Debug.Log(res);
			Debug.Log(scriptData.BaseData);
			// Map.curr.info.SetSaveable();put here the info file
			// Map.curr.bricks.SeSaveable();put here the bricks file
			cb();
			});
	}
	public static string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
                     //We are limiting the array to 80 so we don't have
                     //to parse the entire html document feel free to 
                     //adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
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
	public static bool IsConnectedToInternet(){
		string HtmlText = GetHtmlFromUri("http://google.com");
		if(HtmlText == "")
		{
         //No connection
			return false;
		}
		else if(!HtmlText.Contains("schema.org/WebPage"))
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
