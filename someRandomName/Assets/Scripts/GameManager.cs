using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text scoreBox;
	[SerializeField]
	private int score;

	private void Start () {
		score = 0;
	}

	public void IncreaseScore () {
		score++;

		scoreBox.text = "Score: " + score.ToString ();
	}
}
