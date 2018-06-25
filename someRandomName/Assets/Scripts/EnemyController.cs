using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	public Transform gmObj;
	private GameManager gm;

	void Start () {
		gm = GameObject.FindObjectOfType<GameManager> ();
	}

	void Update () {
		transform.position -= transform.forward * speed * Time.deltaTime;
	}

	private void OnCollisionEnter (Collision other) {
		if (other.transform.name.Contains ("Bullet")) {
			EnemyHit ();
		}
	}

	private void EnemyHit() {
		gm.IncreaseScore ();
		Destroy (this.gameObject);
	}
}
