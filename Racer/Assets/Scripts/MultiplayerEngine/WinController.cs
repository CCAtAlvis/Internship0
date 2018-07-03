using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkIdentity))]
public class WinController : NetworkBehaviour {
	private Text disp;

	void Start () {
		disp = GameObject.FindObjectOfType<Text> ();
		// Debug.Log ("hello world");
//		Debug.Log (client);
//		Debug.Log (this.connectionToClient.connectionId);
	}

	public void PlayerWon (int connID) {
		Debug.Log ("from PlayerWon: " + connID);
		CmdPlayerWon (connID);
	}

	[Command]
	public void CmdPlayerWon (int connID) {
		Debug.Log("A Client called a command, client's id is: " + connectionToClient);
		RpcPlayerWon (connID);
		// Debug.Log (this.gameObject.name);
	}
	
	[ClientRpc]
	void RpcPlayerWon (int connID) {
		Debug.Log ("hey jude " + connectionToClient.connectionId);

		if (!(isLocalPlayer || isClient))
			return;

		string str;
		Debug.Log (connectionToClient.connectionId);

		if (connID == connectionToClient.connectionId)
			str = "You Won!!!";
		else
			str = "You Lost:(";

		Debug.Log (str);
		// disp.text = str;
	}
}
