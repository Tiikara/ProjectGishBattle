using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour
{

	public GameObject PlayerInfoPrefab;
	public Vector3 PlayerInfoPosition;

	[HideInInspector]
	public PlayerInfo playerInfo;

	public Texture InventoryTexture;

	[HideInInspector]
	public Inventory inventory;

	public float maxSpeed = 10f; 
	private bool isFacingRight = true;
	private Animator anim;
	private bool isGrounded = false;

	[HideInInspector]
	public float curMaxSpeed;

	private Vector3 minScale;

	float curhealth;

	float move = 0;

	float lastTimeHealth;

	void Awake()
	{
		GameObject objectPlayerInfo = Instantiate(PlayerInfoPrefab) as GameObject;
		playerInfo = objectPlayerInfo.GetComponent<PlayerInfo>();
	}
	
	private void Start()
	{
		curMaxSpeed = maxSpeed;

		anim = GetComponent<Animator>();

		if (networkView.isMine) 
		{
			networkView.RPC ("UpdateNickName", RPCMode.OthersBuffered, playerInfo.GetNickname());
			inventory = new Inventory();

			inventory.SpriteInventory = InventoryTexture;
		}

		minScale = new Vector3(1.756218f,1.756218f,1f);

		health = 0.25f;

		lastTimeHealth = Time.time;
	}

	private void Update()
	{
		if (networkView.isMine) 
		{
			inventory.updateKeyboard ();
			inventory.procBonusStart (this);
		}


		if (Mathf.Abs (rigidbody2D.velocity.y) > 0) 
		{
			isGrounded = false;
		} 
		else 
		{
			isGrounded = true;
		}

		anim.SetBool ("Ground", isGrounded);

		if (networkView.isMine) {
						if (isGrounded) {
								move = Input.GetAxis ("Horizontal");
				
								rigidbody2D.velocity = new Vector2 (move * curMaxSpeed, rigidbody2D.velocity.y);

								if (Input.GetKeyDown (KeyCode.Space)) {
										rigidbody2D.AddForce (new Vector2 (0, 600));				
								}
						}
				} else
						SyncedMovement ();

		playerInfo.transform.position = transform.position + PlayerInfoPosition;

		anim.SetFloat("Speed", Mathf.Abs(move));

		if (move > 0 && !isFacingRight)
				Flip ();
		else if (move < 0 && isFacingRight)
				Flip ();

		if (networkView.isMine && Time.time - lastTimeHealth > 0.5f) 
		{
			health -= 0.005f;
			lastTimeHealth = Time.time;
		}

		if(networkView.isMine)
			inventory.procBonusEnd (this);
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
	
		theScale.x *= -1;

		transform.localScale = theScale;
	}

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	float syncHealth;

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody2D.transform.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = rigidbody2D.velocity;
			stream.Serialize(ref syncVelocity);

			stream.Serialize(ref move);
			stream.Serialize(ref curhealth);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);

			stream.Serialize(ref move);
			syncHealth = health;
			stream.Serialize(ref syncHealth);

			health = syncHealth;

			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody2D.transform.position;
		}
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody2D.transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	public void Increase()
	{
		health +=  0.1f;
	}

	public float health
	{
		set 
		{ 
			if(value > 0.99f)
				curhealth = 0.99f;
			else if(value < 0f)
				curhealth = 0f;
			else
				curhealth = value;

			float sign;

			if(transform.localScale.x > 0f)
				sign = 1f;
			else
				sign = -1f;

			transform.localScale = new Vector3(minScale.x*(curhealth+1f)*sign,minScale.y*(curhealth+1f),minScale.z);
			playerInfo.SetHealth (health);
		}

		get { return curhealth; }
	}

	[RPC]
	void UpdateNickName(string newNickname)
	{
		playerInfo.SetNickname (newNickname);
	}

	void OnGUI()
	{
		if (networkView.isMine) 
		{
			inventory.DrawGUI ();
		}
	}
}
