using UnityEngine;
using UnityEngine.Networking;

public class WinLevel : NetworkBehaviour {

	[SyncVar]
	private GameObject player = null;

	private void OnTriggerEnter (Collider other) {
		if (player != null)
			return;

		player = other.gameObject;
		player.GetComponent<PlayerController> ().CmdSetWinner (other.gameObject.name);
	}
}
