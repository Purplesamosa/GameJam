using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public enum EnemyType
	{
		Ranged,
		Melee,
		Boss
	}

	public EnemyType MyType;
	public int World;
	public int ViewRange;

	private int Health;
	private bool bSawPlayer;
	private static Transform PlayerTransform;

	// Use this for initialization
	void Start ()
	{
		Health =  2 * World * (int)MyType;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!PlayerTransform)
		{
			PlayerManager pm = GameObject.FindObjectOfType<PlayerManager>();
			if(pm)
			{
				PlayerTransform = pm.transform;
			}
			else
			{
				return;
			}
		}

		if(Vector2.SqrMagnitude(PlayerTransform.position - transform.position) <= ViewRange*ViewRange)
		{
			//Walk towards the player
		}
	}
}

