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

using System.Collections;
using UnityEngine;

namespace Facebook.Unity.Example
{
    internal class GraphRequest : MenuBase
    {
        private string apiQuery = string.Empty;
        private Texture2D profilePic;

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && FB.IsLoggedIn;
            if (Button("Basic Request - Me"))
            {
                FB.API("/me", HttpMethod.GET, HandleResult);
            }

            if (Button("Retrieve Profile Photo"))
            {
                FB.API("/me/picture", HttpMethod.GET, ProfilePhotoCallback);
            }

            if (Button("Take and Upload screenshot"))
            {
                StartCoroutine(TakeScreenshot());
            }

            LabelAndTextField("Request", ref apiQuery);
            if (Button("Custom Request"))
            {
                FB.API(apiQuery, HttpMethod.GET, HandleResult);
            }

            if (profilePic != null)
            {
                GUILayout.Box(profilePic);
            }

            GUI.enabled = enabled;
        }

        private void ProfilePhotoCallback(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
            {
                profilePic = result.Texture;
            }

            HandleResult(result);
        }

        private IEnumerator TakeScreenshot()
        {
            yield return new WaitForEndOfFrame();

            var width = Screen.width;
            var height = Screen.height;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            byte[] screenshot = tex.EncodeToPNG();

            var wwwForm = new WWWForm();
            wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
            wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
            FB.API("me/photos", HttpMethod.POST, HandleResult, wwwForm);
        }
    }
}
