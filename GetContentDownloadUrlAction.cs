/* MIT License
 *
 * Copyright (c) 2016 Sathra Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections;
using HutongGames.PlayMaker;
using PlayFab.ClientModels;
using PlayFab;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class GetContentDownloadUrlAction : FsmStateAction {

	[Tooltip("Key of the content item to fetch, usually formatted as a path, e.g. images/a.png")]
	public FsmString key;

	[Tooltip("HTTP method to fetch item - GET or HEAD. Use HEAD when only fetching metadata. Default is GET.")]
	public FsmString httpMethod;

	[Tooltip("True if download through CDN. CDN provides better download bandwidth and time. However, if you want latest, non-cached version of the content, set this to false. Default is true.")]
	public FsmBool thruCDN;

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		base.OnEnter ();

		var request = new GetContentDownloadUrlRequest () {
			Key = key.Value,
		};

		if (!httpMethod.IsNone) {
			request.HttpMethod = httpMethod.Value;
		}

		if (!httpMethod.IsNone) {
			request.ThruCDN = thruCDN.Value;
		}

		PlayFabClientAPI.GetContentDownloadUrl(request, OnSucces, OnError);
	}

	private void OnSucces (GetContentDownloadUrlResult result) {
		Fsm.Event(successEvent);
	}

	private void OnError(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}