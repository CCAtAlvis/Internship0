using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	// as we are having multiple instances of controlled prefabs,
	// each one of them is controlled on every input action.
	// of either:
	// 1. in the the controller script add a check for local player "isLocalPlayer".
	// or 2. use a player setup script like this one.

	// Disable all the controller scripts and all the things that can cause potential error.
	// Add all the things that are to be diabled when the player is initialized
	// Also add the player camera (main) and audio listner attached to it
	[SerializeField]
	public Behaviour[] thingsToDisable;

	public int connID;
	public int playerID;

	void Start () {
		connID = connectionToClient.connectionId;

		if (isLocalPlayer)
			return;

		for (int i = 0; i < thingsToDisable.Length; i++)
			thingsToDisable [i].enabled = false;
	}
}
