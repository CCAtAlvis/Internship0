using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
	public Text livesBox;
	public int lives;
	private int life;
	private  PlayerController pc;

	public GameObject gos;

	void Start () {
		lives = 3;
		life = lives;
		pc = GameObject.FindObjectOfType<PlayerController> ();
	}

	private void OnTriggerExit (Collider other) {
		if ("Enemy" == other.gameObject.tag)
			DecreaseLife ();
	}

	private void DecreaseLife () {
		life--;
		livesBox.text = "Lives: " + life.ToString () + "/" + lives.ToString ();

		if (0 == life) {
			Time.timeScale = 0;
			Destroy (pc.gameObject);
			gos.SetActive (true);
		}
	}
}
