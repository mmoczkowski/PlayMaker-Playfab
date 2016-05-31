using HutongGames.PlayMaker;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class GetUserInventoryAction : FsmStateAction {

	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	[ArrayEditor(VariableType.Unknown)]
	public FsmArray inventory;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var request = new GetUserInventoryRequest();
		
		PlayFabClientAPI.GetUserInventory(request, OnSucces, OnFailure);
	}

	private void OnSucces (GetUserInventoryResult result) {
		inventory.Values = result.Inventory.ToArray();

		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;

		Fsm.Event(failureEvent);
	}
}

}