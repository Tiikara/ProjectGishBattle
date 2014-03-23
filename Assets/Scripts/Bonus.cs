using UnityEngine;
using System.Collections;

public abstract class Bonus : ScriptableObject {

	public Texture SpriteBonus;

	protected float activeTime;
	float startTime = 0.0f;

	public void Activate ()
	{
		startTime = Time.time;
	}

	public abstract void start (PlayerObject player);
	public abstract void end (PlayerObject player);

	public bool isEnd()
	{
		if (Time.time - startTime > activeTime)
						return true;

		return false;
	}

	public void DrawGUI(Rect rectangle)
	{
		GUI.DrawTexture (rectangle, SpriteBonus);
	}
}
