using HutongGames.PlayMaker;
using PlayFab.ClientModels;
using PlayFab;


namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class RedeemCouponAction : FsmStateAction {

	public FsmString couponCode;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	[ArrayEditor(VariableType.Unknown)]
	public FsmArray grantedItems;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var request = new RedeemCouponRequest() { CouponCode = couponCode.Value };
		
		PlayFabClientAPI.RedeemCoupon(request, OnSucces, OnFailure);
	}

	private void OnSucces (RedeemCouponResult result) {
		grantedItems.Values = result.GrantedItems.ToArray();

		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;

		Fsm.Event(failureEvent);
	}
}

}