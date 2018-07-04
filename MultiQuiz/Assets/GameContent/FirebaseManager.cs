using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour {
	// TODO: make sign-up with google!

	private bool canUseFirebase = false;
	DatabaseReference DBref;
	string deviceInfo;
	User user;
	public Text temp;

	// Use this for initialization
	void Start () {
		temp.text = "hello there";

		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				// Set a flag here indiciating that Firebase is ready to use by your application.
				canUseFirebase = true;
				// GoogleSignIn ();
			} else {
				Debug.LogError(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				temp.text = "Could not resolve all Firebase dependencies:";
				// Firebase Unity SDK is not safe to use here.

				// Show a pop-up saying to upgrade Google Play Services
				// and exit the application or redirect to play store
			}
		});

		if (!canUseFirebase)
			return;

		FirebaseApp app = FirebaseApp.DefaultInstance;
		// Set up the Editor before calling into the realtime database.
		app.SetEditorDatabaseUrl ("https://multiquiz-5e71b.firebaseio.com/");
		if (app.Options.DatabaseUrl != null)
			app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);

		// Get the root reference location of the database.
		DBref = FirebaseDatabase.DefaultInstance.GetReference ("/");
//		if(DBref 

		// handle and get user email from Google login
		// from that get user token etc.
		// for now using "SystemInfo.deviceUniqueIdentifier"

		deviceInfo = SystemInfo.deviceUniqueIdentifier;
//		Debug.Log (deviceInfo);
		user = new User ("CC", "test@test", GetUserID ());
		string json = JsonUtility.ToJson(user);
		temp.text = json;

		//TODO: first check if a user already exist with the following key
		// if not, then only create the new user
		// else fetch the user details.

		DBref.Child ("Users").Child (GetUserID ()).SetRawJsonValueAsync (json).ContinueWith (task => {
			if (task.IsFaulted) {
				Debug.Log ("error");
				// TODO: throw and handle error!	
			} else if (task.IsCompleted) {
				Debug.Log ("new user created, user info: " + json);
				temp.text = "new user created, user info: " + json;
			}
		});
	}


	public void AddUserInQueue (string topic, long timestamp = 0) {
		if (!canUseFirebase || DBref == null)
			return;

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

		DBref.Child ("Matches").Child (topic).OrderByChild ("timestamp").LimitToFirst (1).StartAt (timestamp)
			.GetValueAsync ().ContinueWith (task1 => {
			if (task1.IsFaulted) {
				// Handle the error...
				Debug.Log ("error fetching data match data");
			} else if (task1.IsCompleted) {
				DataSnapshot snapshot = task1.Result;
				Debug.Log (snapshot);

//				if snapshop.Value == null => there is no node avaliable
//				so create a new node
//				Debug.Log (snapshot.Value);

//				create a reference to current match node
				DatabaseReference matchRef = DBref.Child ("Matches").Child (topic);

				if (snapshot.Value == null) {
					Debug.Log ("condition 1 check");
					Match newMatch = new Match(GetUserID (), topic);
					string json = JsonUtility.ToJson (newMatch);

					matchRef = matchRef.Child (GetUserID ());
					matchRef.SetRawJsonValueAsync (json).ContinueWith (task2 => {
						if (task2.IsFaulted) {
							Debug.Log ("error");
							// TODO: throw and handle error!	
						} else if (task2.IsCompleted) {
							// handle all important things here
							Debug.Log ("created a new match: " + json);

//							add value change listner to the match reference node
							matchRef.ValueChanged += MatchCheckListner;
						}
					});
				} else {
					Debug.Log ("condition 2 check");
//					Debug.Log (snapshot.Value);

					Dictionary<string, object> snap = snapshot.Value as Dictionary <string, object>;
					Dictionary<string, object> match = new Dictionary<string, object> ();

//					there will only be one entry in the snapshot as we are limiting our search to 1st node only
					foreach (var entry in snap)
						match = snap[entry.Key] as Dictionary<string, object>;

					foreach (var entry in match)
						Debug.Log (entry.Key +" : "+ entry.Value);
					
//					matchRef = matchRef.Child (GetUserID ());
//					add value change listner to the match reference node
//					matchRef.ValueChanged += MatchCheckListner;
				}
			}
		});
	}

	private void MatchCheckListner (object sender, ValueChangedEventArgs args) {
//		sender comes as null value.. dunno why?? :(
//		Debug.Log ("this is sender: " + sender);

		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}

		DataSnapshot ds = args.Snapshot;
		if (ds.Value == null)
			return;

		Dictionary<string, object> snap = ds.Value as Dictionary <string, object>;

		if ((snap ["check"] as string) == "1" && (snap ["otherPlayer"] as string) != "") {
			Match newMatch = new Match (snap, "2");
			string json = JsonUtility.ToJson (newMatch);
			Debug.Log ("updating match: " + json);
			DBref.Child ("Matches").Child ((snap ["topic"] as string)).Child (GetUserID ()).SetRawJsonValueAsync (json);
		}
	}

	private string GetUserID() {
		return deviceInfo;
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
	public string check = "0";
	public int timestamp;

	private DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	public Match () {}

	public Match (string _owner, string _topic) {
		this.owner = _owner;
		this.topic = _topic;
		this.timestamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	}

	public Match(Dictionary <string, object> _match, string _check) {
		this.owner = _match["owner"] as string;
		this.otherPlayer = _match["otherPlayer"] as string;
		this.topic = _match["topic"] as string;
		this.check = _check;
		this.timestamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	}
}
