using UnityEngine;
using UnityEngine.Networking;

public class WinCondition : MonoBehaviour {

	GameObject collidedObject = null;

	private void OnTriggerEnter (Collider other) {
		// Debug.Log (other.gameObject + other.gameObject.name);
		if (collidedObject != null)
			return;

		collidedObject = other.gameObject;
		collidedObject.name = "Winner";
		// Debug.Log (other.gameObject.name);

		int connID = collidedObject.GetComponent<NetworkIdentity> ().connectionToClient.connectionId;
		Debug.Log (connID);
		collidedObject.GetComponent<WinController> ().PlayerWon (connID);
	}
}
