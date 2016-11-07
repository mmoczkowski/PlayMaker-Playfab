using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;

[Tooltip("Signs the user into the PlayFab account, returning a session identifier that can subsequently be used for API calls which require an authenticated user")]
[ActionCategory("PlayFab")]
public class LoginWithEmailAddressAction : FsmStateAction {

	[Tooltip("Unique identifier for the title, found in the Settings > Game Properties section of the PlayFab developer site when a title has been selected")]
	public FsmString titleId;

	[Tooltip("Email address for the account.")]
	public FsmString email;

	[Tooltip("Password for the PlayFab account (6-30 characters).")]
	public FsmString password;

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[Tooltip("Player's unique PlayfabId")]
	[UIHint(UIHint.Variable)]
	public FsmString playFabId;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var request = new LoginWithEmailAddressRequest {
			TitleId = titleId.Value,
			Email = email.Value,
			Password = password.Value,
		};

		PlayFabClientAPI.LoginWithEmailAddress(request, OnSucces, OnFailure);
	}

	private void OnSucces (LoginResult result) {
		playFabId.Value = result.PlayFabId;
		
		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;

		Fsm.Event(failureEvent);
	}
}
