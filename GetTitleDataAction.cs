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

using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class GetTitleDataAction : FsmStateAction {

	public FsmString key;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString data;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter() {
		var keys = new List<string>();
		keys.Add(key.Value);

		var request = new GetTitleDataRequest();
		request.Keys = keys;

		PlayFabClientAPI.GetTitleData(request, OnSucces, OnFailure);
	}

	private void OnSucces(GetTitleDataResult result) {
		string value = null;

		if (result.Data != null) {
			if (result.Data.TryGetValue(key.Value, out value)) {
				data.Value = value;
			}
		}

		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		Debug.LogError(error.ErrorMessage);

		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}