using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseManager : MonoBehaviour {
	// TODO: make sign-up with google!

	private bool canUseFirebase = false;
	DatabaseReference DBref;
	string deviceInfo;
	User user;

	// Use this for initialization
	void Start () {
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				// Set a flag here indiciating that Firebase is ready to use by your application.
				canUseFirebase = true;
				// GoogleSignIn ();
			} else {
				UnityEngine.Debug.LogError(System.String.Format(
					"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.

				// Show a pop-up saying to upgrade Google Play Services
				// and exit the application or redirect to play store
			}
		});

		if (!canUseFirebase)
			return;

		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://multiquiz-5e71b.firebaseio.com/");
		// Get the root reference location of the database.
		DBref = FirebaseDatabase.DefaultInstance.RootReference;

		// handle and get user email from Google login
		// from that get user token etc.
		// for now using "SystemInfo.deviceUniqueIdentifier"

		deviceInfo = SystemInfo.deviceUniqueIdentifier;
//		Debug.Log (deviceInfo);
		user = new User ("CC", "test@test", deviceInfo);
		string json = JsonUtility.ToJson(user);

		//TODO: first check if a user already exist with the following key
		// if not, then only create the new user
		// else fetch the user details.

		DBref.Child ("Users").Child (deviceInfo).SetRawJsonValueAsync (json).ContinueWith (task => {
			if (task.IsFaulted) {
				Debug.Log ("error");
				// TODO: throw and handle error!	
			} else if (task.IsCompleted) {
				Debug.Log ("new user created, user info: " + json);
			}
		});
	}


	/// <summary>
	/// Adds the user in queue.
	/// </summary>
	/// <param name="topic">Topic.</param>
	public void AddUserInQueue (string topic) {
//		get the top most node from that topic
//		check whether the node is active (i.e. the other player is still online)
//		if the other player is online start game
//		else delete that node and call the function again
//		if there are no nodes, create a new one
//		if the topmost node is from same player reset the node
//		
//		checking if node is active:
//		when the node is first created set the check field to 0
//		when other player joins they set it to 1
//		and then to confirm the owner sets the check field to 2

		DBref.Child ("Matches").Child (topic).LimitToFirst (10).GetValueAsync ().ContinueWith (task1 => {
			if (task1.IsFaulted) {
				// Handle the error...
				Debug.Log ("error fetching data match data");
			} else if (task1.IsCompleted) {
				DataSnapshot snapshot = task1.Result;
				// Do something with snapshot...
				Debug.Log (snapshot);

//				if snapshop.Value == null => there is no node avaliable
//				so create a new node
				Debug.Log (snapshot.Value);

				if (snapshot.Value == null) {
					Match newMatch = new Match(deviceInfo, topic);
					string json = JsonUtility.ToJson (newMatch);

					DatabaseReference matchRef = DBref.Child ("Matches").Child (topic).Child (deviceInfo);
					matchRef.SetRawJsonValueAsync (json).ContinueWith (task2 => {
						if (task2.IsFaulted) {
							Debug.Log ("error");
							// TODO: throw and handle error!	
						} else if (task2.IsCompleted) {
							// handle all important things here
							Debug.Log ("created a new match" + json);

//							now add a listner on that node
							matchRef.ValueChanged += MatchCheckListner;
						}
					});
				}
			}
		});
	}

	private void MatchCheckListner (object sender, ValueChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		DataSnapshot ds = args.Snapshot;
		Debug.Log (ds);
	}
}

public class User {
	// decleare all the user attributes here
	public string username;
	public string email;
	public string uniqueId;
	public string profilePicLink;
	public int level = 0;
	public int xp = 0;

	public User () {}

	public User (string _username, string _email, string _id, string _PPlink = "default") {
		this.username = _username;
		this.email = _email;
		this.uniqueId = _id;
		this.profilePicLink = _PPlink;
	}
}

public class Match {
	public string owner;
	public string otherPlayer = null;
	public string topic;
	public int check = 0;

	public Match () {}

	public Match (string _owner, string _topic) {
		this.owner = _owner;
		this.topic = _topic;
	}

	public Match(Match _match, int _check) {
		this.owner = _match.owner;
		this.otherPlayer = _match.otherPlayer;
		this.topic = _match.topic;
		this.check = _check;
	}
}
