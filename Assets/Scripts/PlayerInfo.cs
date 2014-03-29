using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	public GameObject nicknameObject;
	public GameObject healthlineObject;
	public GameObject valueObject;

	private TextMesh nickname;
	private SpriteRenderer lifeline;
	private TextMesh value;

	Vector3 standartPosLifeline;
	Vector3 standartScaleLifeline;

	void Awake()
	{
		nickname = nicknameObject.GetComponent<TextMesh>();
		lifeline = healthlineObject.GetComponent<SpriteRenderer> ();
		value = valueObject.GetComponent<TextMesh> ();

		//nickname = GetComponentInChildren<TextMesh>();
		//lifeline = GetComponentInChildren<SpriteRenderer>();
		//value = GetComponentInChildren<TextMesh>();

		standartPosLifeline = lifeline.transform.localPosition;
		standartScaleLifeline = lifeline.transform.localScale;
	}

	void Start()
	{

	}
	
	public void SetNickname( string nick )
	{
		nickname.text = nick;
	}

	public string GetNickname()
	{
		return nickname.text;
	}

	public void SetHealth( float health )
	{
		value.text = ((int)(health * 100)).ToString ();
		lifeline.transform.localScale = new Vector3 (standartScaleLifeline.x*health, standartScaleLifeline.y, standartScaleLifeline.z);
		float a = (1.0f - health)/2.0f;
		lifeline.transform.localPosition = new Vector3(standartPosLifeline.x-a, standartPosLifeline.y,standartPosLifeline.z);
	}
}
