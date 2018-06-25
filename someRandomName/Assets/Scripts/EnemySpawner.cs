using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public Transform playerObj;
	private float probTime;
	private GameObject clone;
	private EnemyController ec;
	private float levelTime;
	public float speed;

	void Start () {
		speed = 10f;
		probTime = 0f;
		levelTime = 0f;
	}

	void Update () {
		float prob = Random.Range (0, 20);
		probTime += Time.deltaTime;
		levelTime += Time.deltaTime;

		if (probTime > 2f && prob > 15) {
			SpawnEnemy ();
			probTime = 0f;
		}

		if (levelTime > 10f) {
			speed += speed * 0.1f;
			levelTime = 0f;
		}
	}

	private void SpawnEnemy () {
		int prob = Random.Range (-2, 2);
		clone = GameObject.Instantiate (enemyPrefab, new Vector3(prob, 1, 55), Quaternion.identity);
		ec = clone.GetComponent<EnemyController> ();
		ec.speed = speed;
	}
}
