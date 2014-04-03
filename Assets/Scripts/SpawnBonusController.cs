using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBonusController : MonoBehaviour {

	public GameObject BonusFood;
	public GameObject[] Bonuses;

	public float respawnTime;

	public float minDistanceSpawn;

	public int maxBonuses;

	private float curTimeRespawn = 0.0f;

	GameObject[] spawns;
	GameObject[] usedSpawns;

	void Start()
	{
		spawns = GameObject.FindGameObjectsWithTag ("RespawnBonus");

		usedSpawns = new GameObject[spawns.Length];
	}

	void Update () 
	{
		if (!Network.isServer)
						return;

		curTimeRespawn += Time.deltaTime;

		if (curTimeRespawn > respawnTime) 
		{
			curTimeRespawn = 0.0f;

			spawnBonus();
		}
	}

	void spawnBonus()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		float curDistance = float.NegativeInfinity;

		int index = 0;

		int countUsedSpawn = 0;

		for (int i=0; i<spawns.Length; i++) 
		{
			if(usedSpawns[i] != null)
			{
				countUsedSpawn++;
				continue;
			}

			foreach(GameObject player in players)
			{
				Vector2 playerPos = player.transform.position;
				Vector2 spawnPos = spawns[i].transform.position;
				float distance = Vector2.Distance(playerPos, spawnPos);
			
				if(distance > curDistance)
				{
					curDistance = distance;
					index = i;
				}
			}
		}

		if (curDistance < minDistanceSpawn ||
		    countUsedSpawn == spawns.Length ||
		    countUsedSpawn == maxBonuses)
						return;

		GameObject objectSpawn;
		
		if(Random.Range(0,100) < 75) // 25 %chance
			objectSpawn = BonusFood;
		else
			objectSpawn = Bonuses[Random.Range(0, Bonuses.Length)];
		
		usedSpawns[index] = Network.Instantiate(objectSpawn,spawns[index].transform.position,Quaternion.identity, 1) as GameObject;

	}
}
