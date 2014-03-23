using UnityEngine;
using System.Collections;

public class Inventory {

	public Rect SpriteRectangle = new Rect(20,20,150,75);

	public Texture SpriteInventory;

	private Bonus[] items = new Bonus[3];

	private Bonus activeBonus;

	private KeyCode[] controls = new KeyCode[3];

	public Inventory()
	{
		controls[0] = KeyCode.Z;
		controls[1] = KeyCode.X;
		controls[2] = KeyCode.C;
	}

	public void updateKeyboard()
	{
		for (int i=0; i<3; i++) 
		{
			if(Input.GetKeyDown(controls[i]))
			{
				if(items[i] != null)
				{
					activeBonus = items[i];
					activeBonus.Activate();

					items[i] = null;
				}
				return;
			}
		}
	}

	public void procBonusStart(PlayerObject player)
	{
		if(activeBonus != null)
		{
			activeBonus.start(player);
		}
	}

	public void procBonusEnd(PlayerObject player)
	{
		if(activeBonus != null)
		{
			activeBonus.end(player);

			if(activeBonus.isEnd())
				activeBonus = null;
		}
	}
	
	public bool AddItem(Bonus newItem)
	{
		for(int i=0;i<3;i++)
		{
			if(items[i] == null)
			{
				items[i] = newItem;
				return true;
			}
		}

		return false;
	}

	public void DrawGUI()
	{
		GUI.DrawTexture (SpriteRectangle, SpriteInventory);

		int i = 0;
		foreach(Bonus item in items)
		{
			if(item != null)
				item.DrawGUI(new Rect(SpriteRectangle.x + 5 + 45*i, SpriteRectangle.y + 20, 40, 40));
			i++;
		}

	}
}
