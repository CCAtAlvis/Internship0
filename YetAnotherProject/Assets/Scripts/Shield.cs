using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	private void OnTriggerEnter (Collider other) {
		if ("Enemy" == other.tag) {
			other.gameObject.GetComponent<EnemyController> ().EnemyHit ();
		}
	}
}
