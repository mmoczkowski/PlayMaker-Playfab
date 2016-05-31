using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class LoginWithIOSDeviceIDAction : FsmStateAction {

	public FsmString titleId;
	public FsmBool createAccount;

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString playFabId;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		PlayFabSettings.TitleId = titleId.Value;

		var request = new LoginWithIOSDeviceIDRequest() {
			TitleId = titleId.Value,
			DeviceId = SystemInfo.deviceUniqueIdentifier,
			DeviceModel = SystemInfo.deviceModel,
			OS = SystemInfo.operatingSystem,
			CreateAccount = true
		};

		PlayFabClientAPI.LoginWithIOSDeviceID (request, OnLoginSucces, OnLoginFailure);
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