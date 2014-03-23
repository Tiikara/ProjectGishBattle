using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBonusController : MonoBehaviour {

	public GameObject[] Bonuses;

	public float respawnTime;

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

			Random.seed = (int)Time.time;
			int indexSpawn = 0;

			int i;

			for(i=0;i<11;i++)
			{
				indexSpawn = Random.Range(0, spawns.Length);

				if(usedSpawns[indexSpawn] == null)
					break;
			}

			if(i == 11)
				return;

			int indexBonus = Random.Range(0, Bonuses.Length);

			usedSpawns[indexSpawn] = Network.Instantiate(Bonuses[indexBonus],spawns[indexSpawn].transform.position,Quaternion.identity, 1) as GameObject;
		}
	}
}
