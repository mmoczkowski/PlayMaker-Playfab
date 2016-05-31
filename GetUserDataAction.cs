using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using System.Collections.Generic;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class GetUserDataAction : FsmStateAction {

	public FsmString key;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString data;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var keys = new List<string> ();
		keys.Add(key.Value);

		var request = new GetUserDataRequest();
		request.Keys = keys;

		PlayFabClientAPI.GetUserData(request, OnSucces, OnFailure);
	}

	private void OnSucces (GetUserDataResult result) {
		data.Value = result.Data[key.Value].Value;
		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}