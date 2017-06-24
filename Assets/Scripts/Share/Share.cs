using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSparks.Api;
using GameSparks.Platforms;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine.UI;

public class Share : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GetComponent<Button> ().onClick.AddListener (delegate {
			doUpload ();
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void doUpload ()
	{
		print ("doUpload started");
		new GetUploadUrlRequest ()
			.Send ((response) => {
			GSData scriptData = response.ScriptData; 
			string url = response.Url; 
			print (url);
//			StartCoroutine (UploadAMap (Map.curr, url));
		});
		print ("doUpload End");
	}

//	//Need a byte representaion of the Map
//	public IEnumerator UploadAMap (Map map, string url)
//	{
//	
//		// Create a Web Form, this will be our POST method's data
//		var form = new WWWForm ();
//		form.AddField ("somefield", "somedata");
//		form.AddBinaryData ("map", map, "testmap","");
//	
//		//POST the screenshot to GameSparks
//		WWW w = new WWW (url, form);
//		yield return w;
//	
//		if (w.error != null) {
//			Debug.Log (w.error);
//		} else {
//			Debug.Log (w.text);
//		}
//	}
}
