using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public Transform playerObj;
	public float speed;

	private float probTime;
	private GameObject clone;
	private EnemyController ec;

	public int poolSize = 10;
	private GameObject[] enemyPool;
	private int enemyPoolIndex;

	private void Start () {
		enemyPool = new GameObject[poolSize];
		enemyPoolIndex = 0;
		probTime = 0f;

		for (int i = 0; i < poolSize; i++) {
			clone = GameObject.Instantiate (enemyPrefab, Vector3.one , Quaternion.identity);
			clone.SetActive (false);
			ec = clone.GetComponent<EnemyController> ();
			ec.index = i;
			enemyPool [i] = clone;
		}
	}

	private void Update () {
		float prob = Random.Range (0, 10);
		probTime += Time.deltaTime;

		if (prob > 8 && probTime > 0.2f) {
			SpawnEnemy ((++enemyPoolIndex) % poolSize);
			probTime = 0f;
		}
	}

	private void SpawnEnemy (int index) {
		int prob = Random.Range (1, 10);
		float x = playerObj.position.x;
		Vector3 pos;

		if (prob > 4) {
			pos = new Vector3 (Random.Range (x-2f, x+2f), Random.Range (12f, 15f), 0);
		} else {
			pos = new Vector3 (Random.Range (-11f, 11f), Random.Range (12f, 15f), 0);
		}

		enemyPool [index].transform.position = pos;
		enemyPool [index].GetComponent<Rigidbody> ().velocity = Vector3.zero;
		enemyPool [index].SetActive (true);
	}

	public void Respawn (int i) {
		enemyPool [i].SetActive (false);
		enemyPool [i].transform.position = Vector3.zero;
		enemyPool [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	public void Destroy () {
		for (int i = 0; i < poolSize; i++) {
			enemyPool [i].SetActive (false);
			enemyPool [i].transform.position = Vector3.zero;
			enemyPool [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}
	}

	private void OnTriggerEnter (Collider other) {
		if ("Enemy" == other.tag) {
			Respawn (other.GetComponent<EnemyController> ().index);
		} else if (other.tag.Contains ("PowerUp")) {
			Destroy (other.gameObject);
		}
	}
}
