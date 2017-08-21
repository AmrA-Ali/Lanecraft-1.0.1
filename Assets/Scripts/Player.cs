using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Core;
using LC.SaveLoad;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static PlayerData Data;

    public static bool Online;
    public static bool Authenticated;

    public static Action CallBack;

    public static void GetReady(Action cb)
    {
        CallBack = cb;
        Online = LC.Online.Online.IsConnectedToInternet();
        LoadPlayer();
        if (Online)
        {
            //Internet found login usually by checking the user data
            GS.GameSparksAvailable += OnGameSparksConnected;
        }
        else
        {
            Authenticated = false;
            //No internet connection, check if he has logged in before 
            //if yes continue to the game
            //if no require internet connection to login once at least
            if (SaveLoadManager.FirstTime)
            {
                //must have internet connection
                Debug.Log("Player: Internet connection is required!");
            }
            else
            {
                //He has logged in before an the login info has been retrieved 
                Debug.Log("Player: Retrieving old login information");
                CallBack();
            }
        }
    }

    public static void LoadPlayer()
    {
        Data = new PlayerData();
        Data.SetSaveable(SaveLoadManager.Load(Data));
    }

    //FB CallBack
    private static void InitCallbackWithoutLogin()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("FB.Init: False");
        }
    }

    //FB CallBack
    private static void InitCallbackWithLogin()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            ShowLoginFbButton();
        }
        else
        {
            Debug.Log("FB.Init: False");
        }
    }

    //FB CallBack
    private static void OnHideUnity(bool isGameShown)
    {
        Time.timeScale = !isGameShown ? 0 : 1;
    }

    //GS CallBack
    private static void OnGameSparksConnected(bool isConnected)
    {
        if (isConnected)
        {
            if (GS.Authenticated)
            {
                //the player is authenticaed
                //fetch the data and activate the FB sdk signal and app start
                Authenticated = true;
                FetchPlayerData();
                if (!FB.IsInitialized)
                {
                    FB.Init(InitCallbackWithoutLogin, OnHideUnity);
                }
                else
                {
                    FB.ActivateApp();
                }
            }
            else
            {
                //the player is not authenticated
                //activate the FB sdk and authenticate the user using FB 
                //Then authenticate him in GS
                Debug.Log("GS.AUTHENTICATED: False");
                if (!FB.IsInitialized)
                {
                    FB.Init(InitCallbackWithLogin, OnHideUnity);
                }
                else
                {
                    FB.ActivateApp();
                    ShowLoginFbButton();
                }
            }
        }
        else
        {
            Debug.Log("GS.Connected: False");
        }
    }

    private static void ShowLoginFbButton()
    {
        var loginFb = Resources.Load<GameObject>("Prefabs/UI/LoginFB");
        var canvas = GameObject.Find("Canvas");
        var button = Instantiate(loginFb, canvas.transform);
        button.transform.localScale = new Vector3(1, 1, 1);
        Loading.StopLoading();
    }

    public static void LoginFb()
    {
        var perms = new List<string> {"public_profile", "email", "user_friends"};
        FB.LogInWithReadPermissions(perms, fbResult =>
        {
            if (FB.IsLoggedIn)
            {
                new FacebookConnectRequest()
                    .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                    .SetSwitchIfPossible(true)
                    .Send(response =>
                    {
                        if (response.HasErrors)
                        {
                            Debug.Log("GS.FacebookConnect: False");
                        }
                        else
                        {
                            Authenticated = true;
                            FetchPlayerData();
                        }
                    });
            }
            else
            {
                Debug.Log("FB.Login: False");
            }
        });
    }

    public static void FetchPlayerData()
    {
        new AccountDetailsRequest()
            .Send(response =>
            {
//                IList<string> achievements = response.Achievements;
//                GSData currencies = response.Currencies;
//                long? currency1 = response.Currency1;
//                long? currency2 = response.Currency2;
//                long? currency3 = response.Currency3;
//                long? currency4 = response.Currency4;
//                long? currency5 = response.Currency5;
//                long? currency6 = response.Currency6;
                GSData externalIds = response.ExternalIds;
//                var location = response.Location;
//                GSData reservedCurrencies = response.ReservedCurrencies;
//                GSData reservedCurrency1 = response.ReservedCurrency1;
//                GSData reservedCurrency2 = response.ReservedCurrency2;
//                GSData reservedCurrency3 = response.ReservedCurrency3;
//                GSData reservedCurrency4 = response.ReservedCurrency4;
//                GSData reservedCurrency5 = response.ReservedCurrency5;
//                GSData reservedCurrency6 = response.ReservedCurrency6;
                Data.Id = response.UserId;
//                GSData virtualGoods = response.VirtualGoods;

                Data.Name = response.DisplayName;
                Data.Fbid = externalIds.GetString("FB");
                Data.Save();
                Ref.Inst.StartCoroutine(GetFbPicture());
                CallBack();
            });
    }

    public static IEnumerator GetFbPicture()
    {
        var www = new WWW("http://graph.facebook.com/" + Data.Fbid + "/picture?width=210&height=210");
        yield return www;
        Texture2D tempPic = new Texture2D(25, 25);
        www.LoadImageIntoTexture(tempPic);
        Data.Fbpic = tempPic;
        Data.Save();
    }

    //Display logged in user name in the top left corner
//    void OnGUI()
//    {
//        if (GS.Authenticated == true)
//        {
//            GUILayout.BeginArea(new Rect(0, 0, Screen.width, 40));
//
//            GUILayout.BeginVertical();
//
//            GUILayout.FlexibleSpace();
//
//            GUILayout.BeginHorizontal();
//
//            GUILayout.Space(10);
//            GUIStyle guis = new GUIStyle();
//            guis.fontSize = 30;
//            guis.normal.textColor = Color.white;
//            GUILayout.Label(DATA.name, guis, GUILayout.Width(500), GUILayout.Height(40));
//
//            GUILayout.EndHorizontal();
//
//            GUILayout.EndVertical();
//
//            GUILayout.EndArea();
//        }
//    }

    public class PlayerData : ISaveable
    {
        public string Name;
        public string Id;
        public string Fbid;
        public Texture2D Fbpic;
        public int Gold;
        public int Xp;

        public PlayerData()
        {
            DefaultValue();
        }

        public void DefaultValue()
        {
            Name = "anonymous";
            Id = "000000000000000000000000";
        }

        public void Save()
        {
            SaveLoadManager.Save(this);
        }

        public string Creator()
        {
            return Name + "\n" + Id;
        }

        public string TextureToString()
        {
            if (Fbpic == null) return "NO-FBPIC";
            // the fb pic problem solved
            return "NO-FBPIC";
//            byte[] b = Fbpic.EncodeToJPG();
//            string x = Encoding.UTF8.GetString(b);
//            return x;
        }

        public void TextureFromString(string s)
        {
            if (s.Equals("NO-FBPIC"))
            {
                Fbpic = null;
                return;
            }
            // there's a problem with /saving/loading the pic
            //so stop that for now
            Fbpic = null;
            ////////////////////////
//            byte[] b = Encoding.UTF8.GetBytes(s);
//            fbpic = new Texture2D(25, 25);
//            fbpic.LoadImage(b);
        }

        public string FullFileName()
        {
            return FILE.Player;
        }

        public string FileName()
        {
            return "NO_NEED";
        }

        public string GetSaveable()
        {
            return "" + Name + "!" + Id + "!" + Fbid + "!" + Gold + "!" + Xp + "!" + TextureToString();
        }

        public void SetSaveable(string s)
        {
            var a = s.Split('!');
            if (a.Length < 6)
            {
                //not all data available, use default values
                DefaultValue();
            }
            else
            {
                Name = a[0];
                Id = a[1];
                Fbid = a[2];
                Gold = int.Parse(a[3]);
                Xp = int.Parse(a[4]);
                TextureFromString(a[5]);
            }
        }
    }
}