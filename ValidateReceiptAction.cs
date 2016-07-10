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
public class ValidateReceiptAction : FsmStateAction {

	[Note("Android only. Signature returned by the Google Play IAB API.")]
	public FsmString signature;
	public FsmString receipt;
	public FsmString currencyCode;
	public FsmInt price;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;
	
#if UNITY_IOS
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
#elif UNITY_ANDROID
	public override void OnEnter () {
		var request = new ValidateGooglePlayPurchaseRequest() { 
			ReceiptJson = receipt.Value, 
			Signature = signature.Value,
			CurrencyCode = currencyCode.Value, 
			PurchasePrice = price.Value};

		PlayFabClientAPI.ValidateGooglePlayPurchase(request, OnSucces, OnFailure);
	}
	
	private void OnSucces (ValidateGooglePlayPurchaseResult result) {
		Fsm.Event(successEvent);
	}
#endif

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}