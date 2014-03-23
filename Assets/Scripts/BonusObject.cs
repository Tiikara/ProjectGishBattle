using UnityEngine;
using System.Collections;

public class BonusObject : MonoBehaviour {

	public string BonusName;

	private Bonus bonus;

	void Start () {
		 bonus = ScriptableObject.CreateInstance(BonusName) as Bonus;
		 bonus.SpriteBonus = GetComponent<SpriteRenderer>().sprite.texture;
	}

	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			if(other.networkView.isMine)
			{
				if(other.gameObject.GetComponent<PlayerObject>().inventory.AddItem(bonus))
				{
					Network.RemoveRPCsInGroup (1);
					Network.Destroy (gameObject);
				}
			}
		}
	}


}
