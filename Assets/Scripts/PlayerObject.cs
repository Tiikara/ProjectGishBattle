using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour
{
	public GameObject nicknameTextMeshPrefab;
	public Vector3 NicknamePosition;

	[HideInInspector]
	public TextMesh nicknameTextMesh;

	private GameObject objectNicknameMesh;

	public float maxSpeed = 10f; 
	private bool isFacingRight = true;
	private Animator anim;
	private bool isGrounded = false;

	float move = 0;

	void Awake()
	{
		GameObject objectNicknameMesh = Instantiate(nicknameTextMeshPrefab) as GameObject;
		nicknameTextMesh = objectNicknameMesh.GetComponent<TextMesh>();
	}

	//void OnDisable()
	//{
	//	Destroy (objectNicknameMesh as Object);
	//}
	
	private void Start()
	{
		anim = GetComponent<Animator>();

		if(networkView.isMine)
			networkView.RPC("UpdateNickName", RPCMode.OthersBuffered, nicknameTextMesh.text);
	}

	private void Update()
	{



		               if (Mathf.Abs (rigidbody2D.velocity.y) > 0) {
						isGrounded = false;
				} else {
			isGrounded = true;
				}

		anim.SetBool ("Ground", isGrounded);
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		if (networkView.isMine) {
						if (isGrounded) {
								move = Input.GetAxis ("Horizontal");
				
								rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

								if (Input.GetKeyDown (KeyCode.Space)) {
										rigidbody2D.AddForce (new Vector2 (0, 600));				
								}
						}
				} else
						SyncedMovement ();

		nicknameTextMesh.transform.position = transform.position + NicknamePosition;

		anim.SetFloat("Speed", Mathf.Abs(move));

		if (move > 0 && !isFacingRight)
				Flip ();
		else if (move < 0 && isFacingRight)
				Flip ();

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
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);

			stream.Serialize(ref move);

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

	[RPC]
	void UpdateNickName(string newNickname)
	{
		nicknameTextMesh.text = newNickname;
	}
}
