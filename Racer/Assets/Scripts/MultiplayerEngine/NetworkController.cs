using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class NetworkController : NetworkManager  {
	// help: https://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html

	List<Transform> spawnPoints = new List<Transform>();
	// private Transform[] spawnPoints = new Transform[];

//	private int playerCount = 0;
//	private int spawnIndex = 0;

	void Start () {
//		autoCreatePlayer = false;
	
		// set the max numbers of players that can connect
		maxConnections = 4;
	}

	public override void OnStartServer() {
//		spawnPoints = GetStartPosition ();
		NetworkStartPosition[] startPosition = GameObject.FindObjectsOfType<NetworkStartPosition> ();
		for (int i = 0; i < startPosition.Length; i++) {
			spawnPoints.Add (startPosition[i].gameObject.transform);
//			Debug.Log (spawnPoints[i]);
		}

//		Debug.Log (GetStartPosition ());
//		Debug.Log (spawnPoints);
	}
		
	public override void OnServerConnect (NetworkConnection conn) {
		// Debug.Log ("Client " + conn.connectionId + " Connected!");
//		GameObject player = (GameObject)Instantiate (playerPrefab, spawnPoints[spawnIndex]);
//		PlayerSetup PS = player.GetComponent<PlayerSetup> ();
//		PS.playerID = playerCount;
//	
//		playerCount++;
//		spawnIndex = (spawnIndex + 1) % maxConnections;
//
//		// NetworkServer.Spawn (player);
//		Network.Instantiate (player);
	}

	public override void OnClientConnect (NetworkConnection conn) {
		// Debug.Log ("my client id is: " + conn.connectionId);
//		Debug.Log (conn.connectionId + " my connection: " + conn);
		// GameObject clone = clien
	}

}
