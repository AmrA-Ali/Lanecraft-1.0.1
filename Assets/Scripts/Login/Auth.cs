using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameSparks.Api;
using GameSparks.Platforms;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
	
	// Include Facebook namespace

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
	}

	void Update ()
	{
		if (GS.Authenticated) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
		}

	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	// Use this for initialization
	void Start ()
	{
//		GS.Initialise (new PlatformBase ()); 
		GS.GameSparksAvailable += OnGameSparksConnected;
		printGUI ("GS-init");
	}

	private void OnGameSparksConnected (bool _isConnected)
	{
		if (_isConnected) {
			printGUI ("GS-connected");
		} else {
			printGUI ("GS-Disconnected");
		}
	}

	public void LoginFB ()
	{
		var perms = new List<string> (){ "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (perms, (FBResult) => {
			if (FB.IsLoggedIn) {
				printGUI ("GS-Logging");
				new GameSparks.Api.Requests.FacebookConnectRequest ()
					.SetAccessToken (AccessToken.CurrentAccessToken.TokenString)
					.SetSwitchIfPossible (true)
					.Send ((response) => {
					if (response.HasErrors) {
						printGUI ("Error");
					} else {
						printGUI ("GS-Loged");
						string displayName = response.DisplayName; 
						printGUI (displayName);
					}
					
				});
				printGUI ("GS-done");
			} else {
				Debug.Log ("User cancelled login");
			}
		});
	}

	public static void printGUI (string x)
	{
		var display = GameObject.FindObjectOfType<Text> ();
		display.text += " " + x;
		
	}

}
