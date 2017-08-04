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
    internal class AppRequests : MenuBase
    {
        private string requestMessage = string.Empty;
        private string requestTo = string.Empty;
        private string requestFilter = string.Empty;
        private string requestExcludes = string.Empty;
        private string requestMax = string.Empty;
        private string requestData = string.Empty;
        private string requestTitle = string.Empty;
        private string requestObjectID = string.Empty;
        private int selectedAction;
        private string[] actionTypeStrings =
        {
            "NONE",
            OGActionType.SEND.ToString(),
            OGActionType.ASKFOR.ToString(),
            OGActionType.TURN.ToString()
        };

        protected override void GetGui()
        {
            if (Button("Select - Filter None"))
            {
                FB.AppRequest("Test Message", callback: HandleResult);
            }

            if (Button("Select - Filter app_users"))
            {
                List<object> filter = new List<object> { "app_users" };

                // workaround for mono failing with named parameters
                FB.AppRequest("Test Message", null, filter, null, 0, string.Empty, string.Empty, HandleResult);
            }

            if (Button("Select - Filter app_non_users"))
            {
                List<object> filter = new List<object> { "app_non_users" };
                FB.AppRequest("Test Message", null, filter, null, 0, string.Empty, string.Empty, HandleResult);
            }

            // Custom options
            LabelAndTextField("Message: ", ref requestMessage);
            LabelAndTextField("To (optional): ", ref requestTo);
            LabelAndTextField("Filter (optional): ", ref requestFilter);
            LabelAndTextField("Exclude Ids (optional): ", ref requestExcludes);
            LabelAndTextField("Filters: ", ref requestExcludes);
            LabelAndTextField("Max Recipients (optional): ", ref requestMax);
            LabelAndTextField("Data (optional): ", ref requestData);
            LabelAndTextField("Title (optional): ", ref requestTitle);

            GUILayout.BeginHorizontal();
            GUILayout.Label(
                "Request Action (optional): ",
                LabelStyle,
                GUILayout.MaxWidth(200 * ScaleFactor));

            selectedAction = GUILayout.Toolbar(
                selectedAction,
                actionTypeStrings,
                ButtonStyle,
                GUILayout.MinHeight(ButtonHeight * ScaleFactor),
                GUILayout.MaxWidth(MainWindowWidth - 150));

            GUILayout.EndHorizontal();
            LabelAndTextField("Request Object ID (optional): ", ref requestObjectID);

            if (Button("Custom App Request"))
            {
                OGActionType? action = GetSelectedOGActionType();
                if (action != null)
                {
                    FB.AppRequest(
                        requestMessage,
                        action.Value,
                        requestObjectID,
                        string.IsNullOrEmpty(requestTo) ? null : requestTo.Split(','),
                        requestData,
                        requestTitle,
                        HandleResult);
                }
                else
                {
                    FB.AppRequest(
                        requestMessage,
                        string.IsNullOrEmpty(requestTo) ? null : requestTo.Split(','),
                        string.IsNullOrEmpty(requestFilter) ? null : requestFilter.Split(',').OfType<object>().ToList(),
                        string.IsNullOrEmpty(requestExcludes) ? null : requestExcludes.Split(','),
                        string.IsNullOrEmpty(requestMax) ? 0 : int.Parse(requestMax),
                        requestData,
                        requestTitle,
                        HandleResult);
                }
            }
        }

        private OGActionType? GetSelectedOGActionType()
        {
            string actionString = actionTypeStrings[selectedAction];
            if (actionString == OGActionType.SEND.ToString())
            {
                return OGActionType.SEND;
            }
            if (actionString == OGActionType.ASKFOR.ToString())
            {
                return OGActionType.ASKFOR;
            }
            if (actionString == OGActionType.TURN.ToString())
            {
                return OGActionType.TURN;
            }
            return null;
        }
    }
}
