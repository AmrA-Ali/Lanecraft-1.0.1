using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSparks.Api;
using GameSparks.Platforms;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparks.Api.Messages;
using UnityEngine.UI;

using System.IO;
using System.Text;

using UnityEngine.Networking;


public class Share : MonoBehaviour
{
	public bool flag = false;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Button> ().onClick.AddListener (delegate {
			doUpload ();
		});

		//We will be passing all our messages to a listener function
		UploadCompleteMessage.Listener += GetUploadMessage;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (flag) {
			print ("uploading...");
		}
	}

	public void doUpload ()
	{
		print ("doUpload started");
		new GetUploadUrlRequest ().Send ((response) => {
			//Start coroutine and pass in the upload url
			StartCoroutine (UploadAFile (Map.curr, response.Url));	
		});
	}

	//Our coroutine takes the upload url and map
	public IEnumerator UploadAFile (Map map, string uploadUrl)
	{
		WWW w = new WWW (map.FileNameBricks ());
		yield return w;

		// Create a Web Form
		var form = new WWWForm ();
		form.AddBinaryData ("file", w.bytes, map.info.code);

		w = new WWW (uploadUrl, form);

		flag = true;

		yield return w;

		flag = false;

		if (w.error != null) {
			Debug.Log (w.error);
		} else {
			Debug.Log (w.text);
		}
	}
	//This will be our message listener
	public void GetUploadMessage (GSMessage message)
	{
		string uploadid = message.BaseData.GetString ("uploadId");
		var info = JsonUtility.ToJson (Map.curr.info);
		new LogEventRequest ().SetEventKey ("MAP_ADD")
			.SetEventAttribute ("info", info)
			.SetEventAttribute ("id", uploadid)
			.Send ((res) => {
			print ("Event Recieved: MAP_ADD");
		});
	}
}
