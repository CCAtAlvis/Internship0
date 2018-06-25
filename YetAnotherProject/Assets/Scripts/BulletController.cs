using UnityEngine;

public class BulletController : MonoBehaviour {
	public float speed = 5;

	// Update is called once per frame
	void Update () {
		transform.position += Vector3.up * speed * Time.deltaTime;

		if (transform.position.y > 20)
			Destroy (this.gameObject);
	}

	private void OnTriggerEnter (Collider other) {
		if ("Enemy" == other.tag) {
			other.GetComponent<EnemyController> ().EnemyHit ();
			Destroy (this.gameObject);
		}
	}
}
