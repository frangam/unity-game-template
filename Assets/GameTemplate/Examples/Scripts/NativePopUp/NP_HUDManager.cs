using UnityEngine;
using System.Collections;

public class NP_HUDManager : MonoBehaviour {


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

	private void OnClosedMessage()
	{
		NativeMobileMessage.Create("Message Closed","Native Message was Closed");
	}
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
}
