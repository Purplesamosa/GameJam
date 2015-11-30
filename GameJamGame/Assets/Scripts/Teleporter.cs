using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
	[HideInInspector]
	public LevelBuilder MyLevelBuilder;

	public SpriteRenderer MyGem;

	public Sprite [] DifferentGems;

	public void SetGem(int _world)
	{
		MyGem.sprite = DifferentGems[_world-1];
	}

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.GetComponent<PlayerManager>())
		{
			MyLevelBuilder.FinishLevel();
		}
	}
}

