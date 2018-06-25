using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject Bullet;
	public GameObject gos;

	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Horizontal")) {
			float val = Input.GetAxis ("Horizontal");
			gameObject.GetComponent<Rigidbody> ().velocity = transform.right * val * 5;
		}

		if (Input.GetButtonDown ("Fire1")) {
			GameObject.Instantiate (Bullet, transform.position + transform.forward*2, transform.rotation);
		}
	}

	private void OnCollisionEnter (Collision other) {
		if ("Enemy" == other.gameObject.tag || "Obstacle" == other.gameObject.tag)
			GameOver ();
	}

	private void OnCollisionExit (Collision other) {
		if ("Ground" == other.transform.name)
			GameOver ();
	}

	private void GameOver () {
		Time.timeScale = 0;
		Destroy (this.gameObject);
		gos.SetActive (true);
	}
}
