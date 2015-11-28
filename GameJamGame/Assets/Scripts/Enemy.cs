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

	private float Speed = 70.0f;
	private int Health;
	private bool bSawPlayer;
	private Animator m_Animator;
	private Rigidbody2D m_RigidBody;
	private static Transform PlayerTransform;
	
	// Use this for initialization
	void Start ()
	{
		Health =  2 * World * (int)MyType;
		m_Animator = GetComponent<Animator>();
		m_RigidBody = GetComponent<Rigidbody2D>();
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
			Vector2 direction = (PlayerTransform.position - transform.position).normalized;
			m_RigidBody.velocity = new Vector2(direction.x * Speed, direction.y * Speed	);
			m_Animator.SetFloat("XVelocity",m_RigidBody.velocity.x);
			m_Animator.SetFloat("YVelocity",m_RigidBody.velocity.y);
		}
	}
}

