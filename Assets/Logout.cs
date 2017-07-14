using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

using Facebook.Unity;

public class Logout : MonoBehaviour {

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { Log_out(); }); }
	
	void Log_out(){
		GS.Reset();
			UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
	}
}
