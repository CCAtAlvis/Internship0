using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FirebaseManager))]
public class GameManager : MonoBehaviour {
	public GameObject startingScreen; //0
	public GameObject settingsScreen; //1
	public GameObject selectTopicScreen; //2
	public GameObject gameScreen; //3

	public FirebaseManager FBM;

	private GameObject currentScreen;
	private GameObject previousScreen;
	private int currentScreenIndex;

	public GameObject prefabButton;
	public RectTransform ParentPanel;

	private void Start () {
		currentScreen = startingScreen;
		currentScreenIndex = 0;
		FBM = gameObject.GetComponent<FirebaseManager> ();
	}

	public void OpenSettings () {
		currentScreen.SetActive (false);
		currentScreen = settingsScreen;
		currentScreen.SetActive (true);
		previousScreen = startingScreen;
	}

	public void OpenStart () {
		currentScreen.SetActive (false);
		currentScreen = startingScreen;
		currentScreen.SetActive (true);
		previousScreen = null;
	}

	public void OpenTopics () {
		currentScreen.SetActive (false);
		currentScreen = selectTopicScreen;
		currentScreen.SetActive (true);
		previousScreen = startingScreen;

		// TODO: get the list of toipcs avaliable and run loop over them
		// considering the name of topic to be string

		for(int i = 1; i < 6; i++)
		{
			GameObject goButton = (GameObject)Instantiate(prefabButton);
			goButton.transform.position = new Vector2 (240, 500 - i * 60);
			goButton.transform.SetParent(ParentPanel);
			goButton.transform.localScale = new Vector3(1, 1, 1);

			Button tempButton = goButton.GetComponent<Button>();
			tempButton.GetComponentInChildren<Text> ().text = "button " + i.ToString ();
			string tempInt = i.ToString ();

			tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
		}
	}

	void ButtonClicked(string buttonNo)
	{
		Debug.Log ("Button clicked = " + buttonNo);
		FBM.AddUserInQueue (buttonNo);

	}

	public void OpenGameScreen () {
		currentScreen.SetActive (false);
		currentScreen = gameScreen;
		currentScreen.SetActive (true);
		previousScreen = selectTopicScreen;
	}

	public void OpenPreviousScreen () {
		currentScreen.SetActive (false);
		currentScreen = previousScreen;
		currentScreen.SetActive (true);
	}
}
