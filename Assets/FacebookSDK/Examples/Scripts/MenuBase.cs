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

using System;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
    internal abstract class MenuBase : ConsoleBase
    {
        private static ShareDialogMode shareDialogMode;

        protected abstract void GetGui();

        protected virtual bool ShowDialogModeSelector()
        {
            return false;
        }

        protected virtual bool ShowBackButton()
        {
            return true;
        }

        protected void HandleResult(IResult result)
        {
            if (result == null)
            {
                LastResponse = "Null Response\n";
                LogView.AddLog(LastResponse);
                return;
            }

            LastResponseTexture = null;

            // Some platforms return the empty string instead of null.
            if (!string.IsNullOrEmpty(result.Error))
            {
                Status = "Error - Check log for details";
                LastResponse = "Error Response:\n" + result.Error;
            }
            else if (result.Cancelled)
            {
                Status = "Cancelled - Check log for details";
                LastResponse = "Cancelled Response:\n" + result.RawResult;
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                Status = "Success - Check log for details";
                LastResponse = "Success Response:\n" + result.RawResult;
            }
            else
            {
                LastResponse = "Empty Response\n";
            }

            LogView.AddLog(result.ToString());
        }

        protected void OnGUI()
        {
            if (IsHorizontalLayout())
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
            }

            GUILayout.Label(GetType().Name, LabelStyle);

            AddStatus();

            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 scrollPosition = ScrollPosition;
                scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                ScrollPosition = scrollPosition;
            }
            #endif
            ScrollPosition = GUILayout.BeginScrollView(
                ScrollPosition,
                GUILayout.MinWidth(MainWindowFullWidth));

            GUILayout.BeginHorizontal();
            if (ShowBackButton())
            {
                AddBackButton();
            }

            AddLogButton();
            if (ShowBackButton())
            {
                // Fix GUILayout margin issues
                GUILayout.Label(GUIContent.none, GUILayout.MinWidth(MarginFix));
            }

            GUILayout.EndHorizontal();
            if (ShowDialogModeSelector())
            {
                AddDialogModeButtons();
            }

            GUILayout.BeginVertical();

            // Add the ui from decendants
            GetGui();
            GUILayout.Space(10);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void AddStatus()
        {
            GUILayout.Space(5);
            GUILayout.Box("Status: " + Status, TextStyle, GUILayout.MinWidth(MainWindowWidth));
        }

        private void AddBackButton()
        {
            GUI.enabled = MenuStack.Any();
            if (Button("Back"))
            {
                GoBack();
            }

            GUI.enabled = true;
        }

        private void AddLogButton()
        {
            if (Button("Log"))
            {
                SwitchMenu(typeof(LogView));
            }
        }

        private void AddDialogModeButtons()
        {
            GUILayout.BeginHorizontal();
            foreach (var value in Enum.GetValues(typeof(ShareDialogMode)))
            {
                AddDialogModeButton((ShareDialogMode)value);
            }

            GUILayout.EndHorizontal();
        }

        private void AddDialogModeButton(ShareDialogMode mode)
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && (mode != shareDialogMode);
            if (Button(mode.ToString()))
            {
                shareDialogMode = mode;
                FB.Mobile.ShareDialogMode = mode;
            }

            GUI.enabled = enabled;
        }
    }
}
