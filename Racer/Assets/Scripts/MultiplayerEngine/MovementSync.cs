using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class MovementSync : NetworkBehaviour {

	private Rigidbody physicsRoot;

	void Start () {
		physicsRoot = GetComponent<Rigidbody> ();
	}

	void Update () {
		if (isLocalPlayer) {
			CmdSyncPos (transform.localPosition, transform.localRotation, physicsRoot.velocity, this.gameObject); 
		}
	}

	// Send position to the server and run the RPC for everyone, including the server. 
	[Command]
	protected void CmdSyncPos (Vector3 localPosition, Quaternion localRotation, Vector3 velocity, GameObject player) {
		RpcSyncPos (localPosition, localRotation, velocity, player);
	}

	// For each player, transfer the position from the server to the client, and set it as long as it's not the local player. 
	[ClientRpc]
	void RpcSyncPos (Vector3 localPosition, Quaternion localRotation, Vector3 velocity, GameObject player) {
		if (!isLocalPlayer) {
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			physicsRoot.velocity = velocity;

			if (this.gameObject != player ) {
				transform.parent = player.transform;
			}
		}
	}}
