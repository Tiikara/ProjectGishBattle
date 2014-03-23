using UnityEngine;
using System.Collections;

public class BonusSpeed : Bonus {
	public BonusSpeed()
	{
		activeTime = 3f;
	}

	public override void start (PlayerObject player)
	{
		player.curMaxSpeed = 20f;
	}

	public override void end (PlayerObject player)
	{
		player.curMaxSpeed = player.maxSpeed;
	}
}
