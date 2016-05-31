using HutongGames.PlayMaker;
using PlayFab.ClientModels;
using System.Collections.Generic;
using PlayFab;


namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class UpdateUserDataAction : FsmStateAction {

	public FsmString key;
	public FsmString data;

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var request = new UpdateUserDataRequest() { 
			Data = new Dictionary<string, string>(){
				{key.Value, data.Value}
		    }
		};

		PlayFabClientAPI.UpdateUserData(request, OnSucces, OnFailure);
	}

	private void OnSucces (UpdateUserDataResult result) {
		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}