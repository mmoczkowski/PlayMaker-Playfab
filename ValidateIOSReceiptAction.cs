using HutongGames.PlayMaker;
using PlayFab.ClientModels;
using PlayFab;


namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class ValidateIOSReceiptAction : FsmStateAction {

	public FsmString receipt;
	public FsmString currencyCode;
	public FsmInt price;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;
	
	public override void OnEnter () {
		var request = new ValidateIOSReceiptRequest() { 
			ReceiptData = receipt.Value, 
			CurrencyCode = currencyCode.Value, 
			PurchasePrice = price.Value};

		PlayFabClientAPI.ValidateIOSReceipt(request, OnSucces, OnFailure);
	}
	
	private void OnSucces (ValidateIOSReceiptResult result) {
		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}