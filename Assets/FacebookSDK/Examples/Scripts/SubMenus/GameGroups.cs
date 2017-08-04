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
using UnityEngine;

namespace Facebook.Unity.Example
{
    internal class GameGroups : MenuBase
    {
        private string gamerGroupName = "Test group";
        private string gamerGroupDesc = "Test group for testing.";
        private string gamerGroupPrivacy = "closed";
        private string gamerGroupCurrentGroup = string.Empty;

        protected override void GetGui()
        {
            if (Button("Game Group Create - Closed"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "CLOSED",
                    HandleResult);
            }

            if (Button("Game Group Create - Open"))
            {
                FB.GameGroupCreate(
                    "Test game group",
                    "Test description",
                    "OPEN",
                    HandleResult);
            }

            LabelAndTextField("Group Name", ref gamerGroupName);
            LabelAndTextField("Group Description", ref gamerGroupDesc);
            LabelAndTextField("Group Privacy", ref gamerGroupPrivacy);

            if (Button("Call Create Group Dialog"))
            {
                CallCreateGroupDialog();
            }

            LabelAndTextField("Group To Join", ref gamerGroupCurrentGroup);
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && !string.IsNullOrEmpty(gamerGroupCurrentGroup);
            if (Button("Call Join Group Dialog"))
            {
                CallJoinGroupDialog();
            }

            GUI.enabled = enabled && FB.IsLoggedIn;
            if (Button("Get All App Managed Groups"))
            {
                CallFbGetAllOwnedGroups();
            }

            if (Button("Get Gamer Groups Logged in User Belongs to"))
            {
                CallFbGetUserGroups();
            }

            GUI.enabled = enabled && !string.IsNullOrEmpty(gamerGroupCurrentGroup);
            if (Button("Make Group Post As User"))
            {
                CallFbPostToGamerGroup();
            }

            GUI.enabled = enabled;
        }

        private void GroupCreateCB(IGroupCreateResult result)
        {
            HandleResult(result);
            if (result.GroupId != null)
            {
                gamerGroupCurrentGroup = result.GroupId;
            }
        }

        private void GetAllGroupsCB(IGraphResult result)
        {
            if (!string.IsNullOrEmpty(result.RawResult))
            {
                LastResponse = result.RawResult;
                var resultDictionary = result.ResultDictionary;
                if (resultDictionary.ContainsKey("data"))
                {
                    var dataArray = (List<object>)resultDictionary["data"];

                    if (dataArray.Count > 0)
                    {
                        var firstGroup = (Dictionary<string, object>)dataArray[0];
                        gamerGroupCurrentGroup = (string)firstGroup["id"];
                    }
                }
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                LastResponse = result.Error;
            }
        }

        private void CallFbGetAllOwnedGroups()
        {
            FB.API(FB.AppId + "/groups", HttpMethod.GET, GetAllGroupsCB);
        }

        private void CallFbGetUserGroups()
        {
            FB.API("/me/groups?parent=" + FB.AppId, HttpMethod.GET, HandleResult);
        }

        private void CallCreateGroupDialog()
        {
            FB.GameGroupCreate(
                gamerGroupName,
                gamerGroupDesc,
                gamerGroupPrivacy,
                GroupCreateCB);
        }

        private void CallJoinGroupDialog()
        {
            FB.GameGroupJoin(
                gamerGroupCurrentGroup,
                HandleResult);
        }

        private void CallFbPostToGamerGroup()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["message"] = "herp derp a post";

            FB.API(
                gamerGroupCurrentGroup + "/feed",
                HttpMethod.POST,
                HandleResult,
                dict);
        }
    }
}
