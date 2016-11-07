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

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class LoginWithDeviceAction : FsmStateAction {

	public FsmString titleId;
	public FsmBool createAccount;

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[Tooltip("Player's unique PlayfabId")]
	[UIHint(UIHint.Variable)]
	public FsmString playFabId;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		PlayFabSettings.TitleId = titleId.Value;

#if UNITY_IOS
		var request = new LoginWithIOSDeviceIDRequest() {
			TitleId = titleId.Value,
			DeviceId = SystemInfo.deviceUniqueIdentifier,
			DeviceModel = SystemInfo.deviceModel,
			OS = SystemInfo.operatingSystem,
			CreateAccount = true
		};

		PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSucces, OnLoginFailure);
#elif UNITY_ANDROID
		var request = new LoginWithAndroidDeviceIDRequest() {
			TitleId = titleId.Value,
			AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
			AndroidDevice = SystemInfo.deviceModel,
			OS = SystemInfo.operatingSystem,
			CreateAccount = true
		};

		PlayFabClientAPI.LoginWithAndroidDeviceID (request, OnLoginSucces, OnLoginFailure);
#elif UNITY_STANDALONE
		var request = new LoginWithCustomIDRequest() {
			TitleId = titleId.Value,
			CustomId = UnityEngine.SystemInfo.deviceUniqueIdentifier,
			CreateAccount = true
		};

	    PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucces, OnLoginFailure);
#endif	
}

	private void OnLoginSucces(LoginResult result) {
		playFabId.Value = result.PlayFabId;

		Fsm.Event(successEvent);
	}

	private void OnLoginFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;

		Fsm.Event(failureEvent);
	}
}

}