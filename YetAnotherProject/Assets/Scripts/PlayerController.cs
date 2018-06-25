using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject Bullet;
	public GameObject gos;
	public GameObject shield;
	public EnemySpawner es;

	public bool powerBullet;
	public bool powerShield;
	public bool powerDestroy;

	private float shieldTime;
	private float bulletTime;
	private float lastBulletTime;

	private void Start () {
		shieldTime = 10f;
		bulletTime = 10f;
		lastBulletTime = 0f;
		shield.SetActive (false);
	}

	// Update is called once per frame
	private void Update () {
		lastBulletTime += Time.deltaTime;

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

		if (Input.GetButton ("Horizontal")) {
			float val = Input.GetAxis ("Horizontal");
			gameObject.GetComponent<Rigidbody> ().velocity = transform.right * val * 10;

			if (screenPos.x < 0 || screenPos.x > Screen.width) {
				gameObject.GetComponent<Rigidbody> ().velocity = -transform.right * val * 5;
			}
		}

		// power-up for bullet
		if (powerBullet && bulletTime > 0f) {
			bulletTime -= Time.deltaTime;
			if (lastBulletTime > 0.2f) {
				GameObject.Instantiate (Bullet, transform.position + transform.up, transform.rotation);
				lastBulletTime = 0f;
			}
		} else if (bulletTime < 0f) {
			powerBullet = false;
		}

		// power-up for shield
		if (powerShield && shieldTime > 0f) {
			shieldTime -= Time.deltaTime;
		} else if (shieldTime < 0f) {
			powerShield = false;
			shield.SetActive (false);
		}
	}

	private void OnCollisionEnter (Collision other) {
		if ("Enemy" == other.gameObject.tag)
			GameOver ();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag.Contains ("PowerUp")) {
			GivePower (other.tag);
			Destroy (other.gameObject);
		}
	}

	public void GivePower (string powerup) {
		switch (powerup) {
			case "PowerUp-Bullet":
				powerBullet = true;
				bulletTime += 10f;
				break;
 
			case "PowerUp-Shield":
				powerShield = true;
				shieldTime += 10f;
				shield.SetActive (true);
				break;

			case "PowerUp-Destroy":
				es.Destroy ();
				break;
		}
	}

	private void GameOver () {
		Time.timeScale = 0;
		// Destroy (this.gameObject);
		gos.SetActive (true);
	}
}
