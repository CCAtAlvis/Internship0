using UnityEngine;

public class EnemyController : MonoBehaviour {
	public int index;
	public EnemySpawner es;

	private void Start () {
		es = GameObject.FindObjectOfType<EnemySpawner> ();
	}

	private void OnCollisionEnter (Collision other) {
		if ("Bullet" == other.transform.tag) {
			EnemyHit ();
		}
	}

	public void EnemyHit() {
		es.Respawn (index);
	}
}
