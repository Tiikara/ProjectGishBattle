using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject PlayerClient;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerClient == null)
			return;

		Vector3 pos = new Vector3 (PlayerClient.transform.position.x, PlayerClient.transform.position.y, -10);

		transform.position = pos;


	}
}
