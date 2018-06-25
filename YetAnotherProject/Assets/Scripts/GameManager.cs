using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public float leveltime;
	private float levelTimer;

	public GameObject puBullet;
	public GameObject puShield;
	public GameObject puDestroy;

	public Text time;

	private float probTime;

	// Use this for initialization
	private void Start () {
		levelTimer = 0f;
		probTime = 0f;
	}
	
	// Update is called once per frame
	private void Update () {
		levelTimer += Time.deltaTime;

		if (levelTimer > leveltime) {
			Physics.gravity += Physics.gravity * 0.01f;
			levelTimer = 0f;
		}

		time.text = "Time: " + ((int)Time.timeSinceLevelLoad).ToString ();

		float prob = Random.Range (0, 10);
		probTime += Time.deltaTime;

		if (prob > 8 && probTime > Random.Range (7f, 4f)) {
			SpawnPowerUp ();
			probTime = 0f;
		}
	}

	public void LoadMainLevel () {
		Time.timeScale = 1;
		SceneManager.LoadScene (1);
	}

	private void SpawnPowerUp () {
		int prob = Random.Range (1, 10);
		GameObject toSpawn;
		Vector3 pos = new Vector3 (Random.Range (-8f, 8f), Random.Range (12f, 15f), 0);

		if (prob <= 5) {
			toSpawn = puBullet;
		} else if (prob <= 8) {
			toSpawn = puShield;
		} else {
			toSpawn = puDestroy;
		}

		GameObject.Instantiate (toSpawn, pos , Quaternion.identity);
	}

}
