using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {

	public GameObject Character;

	GameObject[] spawnPoints;

	void Start()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");
	}

	void OnPlayerDisconnected(NetworkPlayer playerID)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject player in players) 
		{
			if(player.GetComponent<NetworkView>().owner == playerID)
			{
				Destroy(player.GetComponent<PlayerObject>().playerInfo.gameObject);
				break;
			}
		}

		Network.RemoveRPCs (playerID);
		Network.DestroyPlayerObjects (playerID);
	}

	void OnServerInitialized()
	{
		InitClient();
	}
	
	void OnConnectedToServer()
	{
		InitClient();
	}
	
	private void InitClient()
	{
		GameObject player = Network.Instantiate(Character, Vector3.zero, Quaternion.identity, 0) as GameObject;
		
		PlayerObject playerObject = player.GetComponent ("PlayerObject") as PlayerObject;

		playerObject.playerInfo.SetNickname 
			(  
			 (GameObject.Find("NetworkManager").GetComponent("NetworkManager") as NetworkManager).nickname
			 );
		
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		
		CameraMovement cameraMove =  camera.GetComponent ("CameraMovement") as CameraMovement;
		
		cameraMove.PlayerClient = player as GameObject;

		//SpawnPlayer (player);
	}

	public void SpawnPlayer(GameObject player)
	{
		Vector2 curSpawnPos = Vector2.zero;

		float curDistance = float.NegativeInfinity;

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		if (players.Length == 1) 
		{
			int spawnId = Random.Range(0, spawnPoints.Length);
			player.transform.position = new Vector3(spawnPoints[spawnId].transform.position.x, spawnPoints[spawnId].transform.position.y, 0f);
			return;
		}

		foreach (GameObject spawn in spawnPoints) 
		{
			foreach(GameObject otherPlayer in players)
			{
				if(otherPlayer == player)
					continue;

				Vector2 playerPos = otherPlayer.transform.position;
				Vector2 spawnPos = spawn.transform.position;
				float distance = Vector2.Distance(playerPos, spawnPos);

				if(distance > curDistance)
				{
					curDistance = distance;
					curSpawnPos = spawnPos;
				}
			}
		}

		player.transform.position = new Vector3(curSpawnPos.x, curSpawnPos.y, 0.0f);

	}
}
