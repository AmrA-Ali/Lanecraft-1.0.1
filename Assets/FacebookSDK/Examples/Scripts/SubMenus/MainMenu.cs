/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
    internal sealed class MainMenu : MenuBase
    {
        protected override bool ShowBackButton()
        {
            return false;
        }

        protected override void GetGui()
        {
            GUILayout.BeginVertical();

            bool enabled = GUI.enabled;
            if (Button("FB.Init"))
            {
                FB.Init(OnInitComplete, OnHideUnity);
                Status = "FB.Init() called with " + FB.AppId;
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = enabled && FB.IsInitialized;
            if (Button("Login"))
            {
                CallFBLogin();
                Status = "Login called";
            }

            GUI.enabled = FB.IsLoggedIn;
            if (Button("Get publish_actions"))
            {
                CallFBLoginForPublish();
                Status = "Login (for publish_actions) called";
            }

            // Fix GUILayout margin issues
            GUILayout.Label(GUIContent.none, GUILayout.MinWidth(MarginFix));
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            // Fix GUILayout margin issues
            GUILayout.Label(GUIContent.none, GUILayout.MinWidth(MarginFix));
            GUILayout.EndHorizontal();

            #if !UNITY_WEBGL
            if (Button("Logout"))
            {
                CallFBLogout();
                Status = "Logout called";
            }
            #endif

            GUI.enabled = enabled && FB.IsInitialized;
            if (Button("Share Dialog"))
            {
                SwitchMenu(typeof(DialogShare));
            }

            bool savedEnabled = GUI.enabled;
            GUI.enabled = enabled &&
                AccessToken.CurrentAccessToken != null &&
                AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions");
            if (Button("Game Groups"))
            {
                SwitchMenu(typeof(GameGroups));
            }

            GUI.enabled = savedEnabled;

            if (Button("App Requests"))
            {
                SwitchMenu(typeof(AppRequests));
            }

            if (Button("Graph Request"))
            {
                SwitchMenu(typeof(GraphRequest));
            }

            if (Constants.IsWeb && Button("Pay"))
            {
                SwitchMenu(typeof(Pay));
            }

            if (Button("App Events"))
            {
                SwitchMenu(typeof(AppEvents));
            }

            if (Button("App Links"))
            {
                SwitchMenu(typeof(AppLinks));
            }

            if (Constants.IsMobile && Button("App Invites"))
            {
                SwitchMenu(typeof(AppInvites));
            }

            if (Constants.IsMobile && Button("Access Token"))
            {
                SwitchMenu(typeof(AccessTokenMenu));
            }

            GUILayout.EndVertical();

            GUI.enabled = enabled;
        }

        private void CallFBLogin()
        {
            FB.LogInWithReadPermissions(new List<string> { "public_profile", "email", "user_friends" }, HandleResult);
        }

        private void CallFBLoginForPublish()
        {
            // It is generally good behavior to split asking for read and publish
            // permissions rather than ask for them all at once.
            //
            // In your own game, consider postponing this call until the moment
            // you actually need it.
            FB.LogInWithPublishPermissions(new List<string> { "publish_actions" }, HandleResult);
        }

        private void CallFBLogout()
        {
            FB.LogOut();
        }

        private void OnInitComplete()
        {
            Status = "Success - Check log for details";
            LastResponse = "Success Response: OnInitComplete Called\n";
            string logMessage = string.Format(
                "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
                FB.IsLoggedIn,
                FB.IsInitialized);
            LogView.AddLog(logMessage);
            if (AccessToken.CurrentAccessToken != null)
            {
                LogView.AddLog(AccessToken.CurrentAccessToken.ToString());
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            Status = "Success - Check log for details";
            LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            LogView.AddLog("Is game shown: " + isGameShown);
        }
    }
}
