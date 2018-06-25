using UnityEngine;

public class BulletController : MonoBehaviour {
	public float speed = 5;

	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * speed * Time.deltaTime;

		if (transform.position.z > 60)
			Destroy (this.gameObject);
	}

	private void OnCollisionEnter (Collision other) {
		Destroy (this.gameObject);
	}
}
