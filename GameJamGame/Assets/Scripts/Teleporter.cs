using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
	[HideInInspector]
	public LevelBuilder MyLevelBuilder;

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.GetComponent<PlayerManager>())
		{
			MyLevelBuilder.FinishLevel();
		}
	}
}

