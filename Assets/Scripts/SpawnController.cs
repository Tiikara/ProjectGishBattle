using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {

	public GameObject Character;

	void OnPlayerConnected(NetworkPlayer aPlayer)
	{

		//networkView.RPC("UpdateNickName", RPCMode.All, nicknameTextMesh.text);
	}

	void OnPlayerDisconnected(NetworkPlayer playerID)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject player in players) 
		{
			if(player.GetComponent<NetworkView>().owner == playerID)
			{
				Destroy(player.GetComponent<PlayerObject>().nicknameTextMesh);
				break;
			}
		}

		Network.RemoveRPCs (playerID);
		Network.DestroyPlayerObjects (playerID);
	}

	void OnServerInitialized()
	{
		spawnPlayer();
	}
	
	void OnConnectedToServer()
	{
		spawnPlayer();
	}
	
	private void spawnPlayer()
	{
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");
		
		GameObject player = Network.Instantiate(Character, spawnPoints[0].transform.position, Quaternion.identity, 0) as GameObject;
		
		PlayerObject playerObject = player.GetComponent ("PlayerObject") as PlayerObject;

		playerObject.nicknameTextMesh.text = 
			(GameObject.Find("NetworkManager").GetComponent("NetworkManager") as NetworkManager).nickname;
		
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		
		CameraMovement cameraMove =  camera.GetComponent ("CameraMovement") as CameraMovement;
		
		cameraMove.PlayerClient = player as GameObject;
	}
}
