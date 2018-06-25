using UnityEngine;

public class PowerUpController : MonoBehaviour {
	private void OnTriggerEnter (Collider other) {
		if ("Player" == other.tag) {
			other.GetComponent<PlayerController> ().GivePower (this.tag);
		}
	}


}
