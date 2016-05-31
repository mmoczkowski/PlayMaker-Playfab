using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class LoginWithGameCenterAction : FsmStateAction {

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

		var request = new LoginWithGameCenterRequest() {
			TitleId = titleId.Value,
			PlayerId = UnityEngine.Social.localUser.id,
			CreateAccount = createAccount.Value
		};

		PlayFabClientAPI.LoginWithGameCenter (request, OnLoginSucces, OnLoginFailure);
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