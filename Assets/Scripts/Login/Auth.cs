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
//	public static Auth curr;

	public Button goToNextButton;
	public Button LoginFBButton;

	//These values shall be loaded from the stoarge with last logged in user values
	public static string NAME = "anonymous";
	public static string UID = "000000000000000000000000";
	public static string FBID = "000000000000000000000000";
	public static Texture2D FBPIC = null;

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
//		curr = this;
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
	}

	public static string Creator ()
	{
		return NAME + "\n" + UID;
	}

	void OnGUI ()
	{
		if (GS.Authenticated == true) {
			GUILayout.BeginArea (new Rect (0, 0, Screen.width, 40));

			GUILayout.BeginVertical ();

			GUILayout.FlexibleSpace ();

			GUILayout.BeginHorizontal ();

			GUILayout.Space (10);
			GUIStyle guis = new GUIStyle ();
			guis.fontSize = 30;
			guis.normal.textColor = Color.white;
			GUILayout.Label (NAME, guis, GUILayout.Width (500), GUILayout.Height (40));

			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			GUILayout.EndArea ();
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
			if (GS.Authenticated) {
				fetchPlayerData ();
			} else {
				LoginFBButton.gameObject.SetActive (true);
			}
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
						fetchPlayerData ();
					}
				});
				printGUI ("GS-done");
			} else {
				Debug.Log ("User cancelled login");
			}
		});
	}

	public IEnumerator GetFBPicture ()
	{
		var www = new WWW ("http://graph.facebook.com/" + FBID + "/picture?width=210&height=210");
		yield return www;
		Texture2D tempPic = new Texture2D (25, 25);
		www.LoadImageIntoTexture (tempPic);
		FBPIC = tempPic;
	}

	public void fetchPlayerData ()
	{
		new AccountDetailsRequest ()
			.Send ((response) => {
			IList<string> achievements = response.Achievements; 
			GSData currencies = response.Currencies; 
			long? currency1 = response.Currency1; 
			long? currency2 = response.Currency2; 
			long? currency3 = response.Currency3; 
			long? currency4 = response.Currency4; 
			long? currency5 = response.Currency5; 
			long? currency6 = response.Currency6; 
			GSData externalIds = response.ExternalIds; 
			var location = response.Location; 
			GSData reservedCurrencies = response.ReservedCurrencies; 
			GSData reservedCurrency1 = response.ReservedCurrency1; 
			GSData reservedCurrency2 = response.ReservedCurrency2; 
			GSData reservedCurrency3 = response.ReservedCurrency3; 
			GSData reservedCurrency4 = response.ReservedCurrency4; 
			GSData reservedCurrency5 = response.ReservedCurrency5; 
			GSData reservedCurrency6 = response.ReservedCurrency6; 
			UID = response.UserId; 
			GSData virtualGoods = response.VirtualGoods; 

			NAME = response.DisplayName; 
			FBID = externalIds.GetString ("FB");
			StartCoroutine (GetFBPicture ());
			goToNextButton.gameObject.SetActive (true);
		});
	}

	public void GoToNextScene ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
	}

	public static void printGUI (string x)
	{
		var display = GameObject.FindObjectOfType<Text> ();
		display.text += " " + x;
		
	}

}
