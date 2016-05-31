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