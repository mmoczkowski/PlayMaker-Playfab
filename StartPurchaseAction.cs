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
using System.Collections.Generic;

namespace Sathra.PlayMaker.PlayFab {

[ActionCategory("PlayFab")]
public class StartPurchaseAction : FsmStateAction {

	public FsmString itemId;
	public FsmInt quantity;
	public FsmString catalogVersion;
	public FsmString storeId;
	public FsmEvent successEvent;
	public FsmEvent failureEvent;

	[UIHint(UIHint.Variable)]
	public FsmString orderId;

	[UIHint(UIHint.Variable)]
	public FsmString errorMessage;

	public override void OnEnter () {
		var itemRequest = new ItemPurchaseRequest() {
			ItemId = itemId.Value,
			Quantity = (uint)quantity.Value
		};

		var items = new List<ItemPurchaseRequest>();
		items.Add(itemRequest);

		var purchaseRequest = new StartPurchaseRequest() {
			Items = items
		};

		if(!catalogVersion.IsNone) {
			purchaseRequest.CatalogVersion = catalogVersion.Value;
		}

		if(!storeId.IsNone) {
			purchaseRequest.StoreId = storeId.Value;
		}

		PlayFabClientAPI.StartPurchase(purchaseRequest, OnSucces, OnFailure);
	}

	private void OnSucces (StartPurchaseResult result) {
		orderId.Value = result.OrderId;
		Fsm.Event(successEvent);
	}

	private void OnFailure(PlayFabError error) {
		errorMessage.Value = error.ErrorMessage;
		Fsm.Event(failureEvent);
	}
}

}