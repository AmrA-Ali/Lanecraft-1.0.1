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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Facebook.Unity.Example
{
    internal class ConsoleBase : MonoBehaviour
    {
        private const int DpiScalingFactor = 160;
        private static Stack<string> menuStack = new Stack<string>();
        private string status = "Ready";
        private string lastResponse = string.Empty;
        private Vector2 scrollPosition = Vector2.zero;

        // DPI scaling
        private float? scaleFactor;
        private GUIStyle textStyle;
        private GUIStyle buttonStyle;
        private GUIStyle textInputStyle;
        private GUIStyle labelStyle;

        protected static int ButtonHeight
        {
            get
            {
                return Constants.IsMobile ? 60 : 24;
            }
        }

        protected static int MainWindowWidth
        {
            get
            {
                return Constants.IsMobile ? Screen.width - 30 : 700;
            }
        }

        protected static int MainWindowFullWidth
        {
            get
            {
                return Constants.IsMobile ? Screen.width : 760;
            }
        }

        protected static int MarginFix
        {
            get
            {
                return Constants.IsMobile ? 0 : 48;
            }
        }

        protected static Stack<string> MenuStack
        {
            get
            {
                return menuStack;
            }

            set
            {
                menuStack = value;
            }
        }

        protected string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        protected Texture2D LastResponseTexture { get; set; }

        protected string LastResponse
        {
            get
            {
                return lastResponse;
            }

            set
            {
                lastResponse = value;
            }
        }

        protected Vector2 ScrollPosition
        {
            get
            {
                return scrollPosition;
            }

            set
            {
                scrollPosition = value;
            }
        }

        // Note we assume that these styles will be accessed from OnGUI otherwise the
        // unity APIs will fail.
        protected float ScaleFactor
        {
            get
            {
                if (!scaleFactor.HasValue)
                {
                    scaleFactor = Screen.dpi / DpiScalingFactor;
                }

                return scaleFactor.Value;
            }
        }

        protected int FontSize
        {
            get
            {
                return (int)Math.Round(ScaleFactor * 16);
            }
        }

        protected GUIStyle TextStyle
        {
            get
            {
                if (textStyle == null)
                {
                    textStyle = new GUIStyle(GUI.skin.textArea);
                    textStyle.alignment = TextAnchor.UpperLeft;
                    textStyle.wordWrap = true;
                    textStyle.padding = new RectOffset(10, 10, 10, 10);
                    textStyle.stretchHeight = true;
                    textStyle.stretchWidth = false;
                    textStyle.fontSize = FontSize;
                }

                return textStyle;
            }
        }

        protected GUIStyle ButtonStyle
        {
            get
            {
                if (buttonStyle == null)
                {
                    buttonStyle = new GUIStyle(GUI.skin.button);
                    buttonStyle.fontSize = FontSize;
                }

                return buttonStyle;
            }
        }

        protected GUIStyle TextInputStyle
        {
            get
            {
                if (textInputStyle == null)
                {
                    textInputStyle = new GUIStyle(GUI.skin.textField);
                    textInputStyle.fontSize = FontSize;
                }

                return textInputStyle;
            }
        }

        protected GUIStyle LabelStyle
        {
            get
            {
                if (labelStyle == null)
                {
                    labelStyle = new GUIStyle(GUI.skin.label);
                    labelStyle.fontSize = FontSize;
                }

                return labelStyle;
            }
        }

        protected virtual void Awake()
        {
            // Limit the framerate to 60 to keep device from burning through cpu
            Application.targetFrameRate = 60;
        }

        protected bool Button(string label)
        {
            return GUILayout.Button(
                label,
                ButtonStyle,
                GUILayout.MinHeight(ButtonHeight * ScaleFactor),
                GUILayout.MaxWidth(MainWindowWidth));
        }

        protected void LabelAndTextField(string label, ref string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, LabelStyle, GUILayout.MaxWidth(200 * ScaleFactor));
            text = GUILayout.TextField(
                text,
                TextInputStyle,
                GUILayout.MaxWidth(MainWindowWidth - 150));
            GUILayout.EndHorizontal();
        }

        protected bool IsHorizontalLayout()
        {
            #if UNITY_IOS || UNITY_ANDROID
            return Screen.orientation == ScreenOrientation.Landscape;
            #else
            return true;
            #endif
        }

        protected void SwitchMenu(Type menuClass)
        {
            menuStack.Push(GetType().Name);
            SceneManager.LoadScene(menuClass.Name);
        }

        protected void GoBack()
        {
            if (menuStack.Any())
            {
                SceneManager.LoadScene(menuStack.Pop());
            }
        }
    }
}
