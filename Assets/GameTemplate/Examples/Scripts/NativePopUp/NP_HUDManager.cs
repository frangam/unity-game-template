using UnityEngine;
using System.Collections;

public class NP_HUDManager : MonoBehaviour {

	private string AndroidUrl = "market://details?id=com.pukepixel.carracingsurvivor";

	public void Message()
	{
		NativeMobileMessage msg = NativeMobileMessage.Create("Title","Native Message Demo");
		msg.OnComplete += OnClosedMessage;
	}

	public void Dialog()
	{
		NativeMobileDialog dialog = NativeMobileDialog.Create("Title", "Native Dialog Demo");
		dialog.OnComplete += OnClosedDialog;
	}

	public void RateUs()
	{
		NativeMobileRateUs rate = NativeMobileRateUs.Create("Rate Us", "If you like this game rate us!", AndroidUrl);
		rate.OnComplete += OnClosedRateUs;
	}
	public void showPreLoader()
	{
		Invoke("HidePreLoader", 2f);
		NativeMobilePreLoader.ShowPreloader("PreLoader Title", "PreLoader Message, hide in 2 seconds...");
	}
	public void HidePreLoader()
	{
		NativeMobilePreLoader.HidePreloader();
	}

	#region EVENTS
	#region Message
	private void OnClosedMessage()
	{
		NativeMobileMessage.Create("Message Closed","Native Message was Closed");
	}
	#endregion
	#region Dialog
	private void OnClosedDialog(NMDialogResult result)
	{
		switch (result) {
		case NMDialogResult.YES:
			NativeMobileMessage.Create("Dialog Closed","User press yes");
			break;
		case NMDialogResult.NO:
			NativeMobileMessage.Create("Dialog Closed","User press no");
			break;

		default:
				break;
		}
	}
	#endregion
	#region RateUS
	private void OnClosedRateUs(NMDialogResult result)
	{
		switch (result) {
		case NMDialogResult.RATED:
			NativeMobileRateUs.OpenAppPage(AndroidUrl);
//			NativeMobileMessage.Create("Dialog Closed","User press Rated");
			break;
		case NMDialogResult.REMIND:
			NativeMobileMessage.Create("Dialog Closed","User press Remind");
			break;
		case NMDialogResult.DECLINED:
			NativeMobileMessage.Create("Dialog Closed","User press Decline");
			break;
		default:
			break;
		}
	}
	#endregion
	#endregion
}
