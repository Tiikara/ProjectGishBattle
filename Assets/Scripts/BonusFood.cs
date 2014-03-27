using UnityEngine;
using System.Collections;

public class BonusFood : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if(other.networkView.isMine)
			{
					other.gameObject.GetComponent<PlayerObject>().Increase();
					Network.RemoveRPCsInGroup (1);
					Network.Destroy (gameObject);
			}
		}
	}
}
