using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using GameSparks.Api;
using GameSparks.Core;
using GameSparks.Platforms;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

using Facebook.Unity;

using LC.SaveLoad;

public class Player : MonoBehaviour
{
	public Button goToNextButton;
	public Button LoginFBButton;
	public GameObject Loading;
	public Text log;

	public static PlayerData DATA;
	
	public static bool ONLINE;
	public static bool READY = false;
	public static bool AUTHENTICATED = false;

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		READY = false;
		StartCoroutine(Init());
	}

	public IEnumerator Init(){
		while(!SaveLoadManager.READY)
		{
			yield return null;
		}

		ONLINE = Online.IsConnectedToInternet();
		DATA = new PlayerData();
		LoadPlayer();

		Debug.Log("Player.ONLINE: "+ONLINE);
		Debug.Log("Player.DATA.GetSaveable: " + DATA.GetSaveable());
		if(ONLINE){//Internet found login usually by checking the user data
			Debug.Log("Player: Connecting to online services");
			GS.GameSparksAvailable += OnGameSparksConnected;
			if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
				FB.Init (InitCallback, OnHideUnity);
			} else {
			// Already initialized, signal an app activation App Event
				FB.ActivateApp ();
			}
		}else{//No internet connection, check if he has logged in before 
			  	//if yes continue to the game
				//if no require internet connection to login once at least
			if(SaveLoadManager.FIRST_TIME){
				//must have internet connection
				Debug.Log("Player: Internet connection is required!");
			}else{
				//He has logged in before an the login info has been retrieved 
				Debug.Log("Player: Retrieving old login information");
				GoToNextScene();
			}
		}
		READY = true;
		Debug.Log("Player.READY: true");
		yield break;	
	}

	public void LoadPlayer(){
		DATA.SetSaveable(SaveLoadManager.Load(DATA));
	}

	//FB CallBack
	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			if (!SaveLoadManager.FIRST_TIME){
				LoginFB();
			}
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}
	
	//FB CallBack
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

	//GS CallBack
	private void OnGameSparksConnected (bool _isConnected)
	{
		if (_isConnected) {
			printGUI ("GS-connected");
			if (GS.Authenticated) {
				fetchPlayerData ();
			} else {

				Loading.SetActive(false);
				LoginFBButton.gameObject.SetActive (true);
			}
		} else {
			printGUI ("GS-Disconnected");
		}
	}

	public void LoginFB ()
	{
		
//			Debug.Log("Access Tokin:"+AccessToken.CurrentAccessToken.TokenString);
		Debug.Log("FB-Is logged in:"+FB.IsLoggedIn);
		var perms = new List<string> (){ "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (perms, (FBResult) => {
			if (FB.IsLoggedIn) {
				printGUI ("FB-Loged in");
				new GameSparks.Api.Requests.FacebookConnectRequest ()
				.SetAccessToken (AccessToken.CurrentAccessToken.TokenString)
				.SetSwitchIfPossible (true)
				.Send ((response) => {
					if (response.HasErrors) {
						printGUI ("GS-Error_logging_in");
					} else {

						printGUI ("GS-Loged_in");
						fetchPlayerData ();
						AUTHENTICATED = true;
					}
					});
			} else {
				Debug.Log ("FB-User cancelled login");
			}
			});
	}

	public IEnumerator GetFBPicture ()
	{
		var www = new WWW ("http://graph.facebook.com/" + DATA.fbid + "/picture?width=210&height=210");
		yield return www;
		Texture2D tempPic = new Texture2D (25, 25);
		www.LoadImageIntoTexture (tempPic);
		DATA.fbpic = tempPic;
		DATA.Save();
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
			DATA.id = response.UserId; 
			GSData virtualGoods = response.VirtualGoods; 

			DATA.name = response.DisplayName; 
			DATA.fbid = externalIds.GetString ("FB");
			DATA.Save();
			StartCoroutine (GetFBPicture ());
			// goToNextButton.gameObject.SetActive (true);
			GoToNextScene();
			});
	}

	public void GoToNextScene ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Main");
	}

	public  void printGUI (string x)
	{
		// log.text += " " + x;
		Debug.Log(x);
	}


	//Display logged in user name in the top left corner
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
			GUILayout.Label (DATA.name, guis, GUILayout.Width (500), GUILayout.Height (40));

			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();

			GUILayout.EndArea ();
		}
	}

	public class PlayerData:Saveable{
		public string name;
		public string id;
		public string fbid;
		public Texture2D fbpic;
		public int gold;
		public int xp;
		
		public PlayerData(){
			name = "anonymous";
			id= "000000000000000000000000";
		}

		public void Save(){
			SaveLoadManager.Save(this);
		}
		public  string Creator ()
		{
			return name + "\n" + id;
		}

		public string TextureToString(){
			if(fbpic == null)return "NO-FBPIC";
			// the fb pic problem solved
			return "NO-FBPIC";
			byte[] b = fbpic.EncodeToJPG();
			string x = Encoding.UTF8.GetString(b);
			return x;
		}

		public void TextureFromString(string s){
			if(s.Equals("NO-FBPIC"))
			{
				fbpic=null;
				return;
			}
			// there's a problem with /saving/loading the pic
			//so stop that for now
			fbpic = null;
			return;
			////////////////////////
			byte[] b = Encoding.UTF8.GetBytes(s);
			fbpic = new Texture2D(25,25);
			fbpic.LoadImage(b);
		}
		public string FullFileName(){
			return FILE.PLAYER;
		}
		public string FileName(){
			return "NO NEED";
		}
		
		public string GetSaveable(){
			return ""+name+"!"+id+"!"+fbid+"!"+gold+"!"+xp+"!"+TextureToString();
		}
		
		public void SetSaveable(string s){
			string[] a = s.Split('!');
			name = a[0];
			id = a[1];
			fbid = a[2];
			gold = int.Parse(a[3]);
			xp = int.Parse(a[4]);
			TextureFromString(a[5]);
		}
	}
}