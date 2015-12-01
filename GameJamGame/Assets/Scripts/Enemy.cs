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
	public float ViewRange = 10.0f;
	public float StopDistance = 0.25f;
	public float Speed = 70.0f;
	public float AttackDelay = 1.0f;
	public float Damage;

	public int XPToGive;

	[HideInInspector]
	public LevelBuilder MyLevelBuilder;
	

	public float Health;
	private bool bSawPlayer = false;
	private Animator m_Animator;
	private Rigidbody2D m_RigidBody;
	private static Transform PlayerTransform;
	private static EnemyBulletManager BulletManager;
	private float AttackTimer = 0.0f;

	private bool m_MeleeAttack = false;

	public AudioClip m_Hurt;
	public AudioClip m_Dead;

	private AudioSource m_AudioSource;

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.transform == PlayerTransform)
		{
			col.gameObject.GetComponent<PlayerManager>().TakeDamage(Damage);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		//Health =  2.0f * World * (int)MyType;
		m_Animator = GetComponent<Animator>();
		m_RigidBody = GetComponent<Rigidbody2D>();
		m_AudioSource = GetComponent<AudioSource>();
	}

	public void TakeDamage(float _dmg)
	{
		Health -= _dmg;
		StartCoroutine(FlashRed());
		if(Health <= 0.0f)
		{
			m_RigidBody.velocity = Vector2.zero;
			m_AudioSource.PlayOneShot(m_Dead);
			GetComponent<EnemyFadeOut>().enabled = true;
			GetComponent<Collider2D>().enabled = false;
			m_Animator.enabled = false;
			enabled = false;
			PlayerTransform.GetComponent<PlayerManager>().GiveXP(XPToGive);
		}
		else
		{
			m_AudioSource.PlayOneShot(m_Hurt);
		}
	}

	private IEnumerator FlashRed()
	{
		GetComponent<SpriteRenderer>().color = Color.red;
		yield return new WaitForSeconds(0.2f);
		GetComponent<SpriteRenderer>().color = Color.white;
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

		if(!BulletManager)
		{
			BulletManager = GameObject.FindObjectOfType<EnemyBulletManager>();
		}

		float dist = Vector2.SqrMagnitude(PlayerTransform.position - transform.position);

		if(dist <= ViewRange*ViewRange || bSawPlayer)
		{
			bSawPlayer = true;
			Vector2 direction = (PlayerTransform.position - transform.position).normalized;
			//Walk towards the player
			if(dist > StopDistance * StopDistance)
			{
				m_RigidBody.velocity = new Vector2(direction.x * Speed * Time.deltaTime, direction.y * Speed * Time.deltaTime);
				m_Animator.SetFloat("XVelocity",m_RigidBody.velocity.x);
				m_Animator.SetFloat("YVelocity",m_RigidBody.velocity.y);
				m_MeleeAttack = false;
			}
			else
			{
				m_RigidBody.velocity = Vector2.zero;
				m_MeleeAttack = true;
			}

			switch(MyType)
			{
			case EnemyType.Melee:
				AttackTimer += Time.deltaTime;
				if(AttackTimer >= AttackDelay && m_MeleeAttack)
				{
					AttackTimer = 0.0f;
					GameplayUIManager.Instance.m_Player.TakeDamage(Damage);
				}
				break;
			case EnemyType.Ranged:
				AttackTimer += Time.deltaTime;
				if(AttackTimer >= AttackDelay)
				{
					AttackTimer = 0.0f;
					BulletManager.FireBullet(transform.position, direction, Damage, false);
				}
				break;
			case EnemyType.Boss:
				AttackTimer += Time.deltaTime;
				if(AttackTimer >= AttackDelay)
				{
					AttackTimer = 0.0f;
					BulletManager.FireBullet(transform.position, direction, Damage, true);

					switch(BulletManager.GetComponent<LevelBuilder>().World)
					{
					case 1:
					{
						Vector2 newvec = new Vector2(direction.x + 0.3f,
						                             direction.y + 0.3f);
						newvec.Normalize();
						BulletManager.FireBullet(transform.position, newvec, Damage, true);

						Vector2 newvec1 = new Vector2(direction.x - 0.3f,
						                             direction.y - 0.3f);
						newvec1.Normalize();
						BulletManager.FireBullet(transform.position, newvec1, Damage, true);
					}
						break;
					case 2:
					{
						Vector2 newvec = new Vector2(direction.x + 0.4f,
						                             direction.y + 0.4f);
						newvec.Normalize();
						BulletManager.FireBullet(transform.position, newvec, Damage, true);
						
						Vector2 newvec1 = new Vector2(direction.x - 0.4f,
						                              direction.y - 0.4f);
						newvec1.Normalize();
						BulletManager.FireBullet(transform.position, newvec1, Damage, true);
						goto case 1;
					}

					case 3:
					{
						Vector2 newvec = new Vector2(direction.x + 0.5f,
						                             direction.y + 0.5f);
						newvec.Normalize();
						BulletManager.FireBullet(transform.position, newvec, Damage, true);
						
						Vector2 newvec1 = new Vector2(direction.x - 0.5f,
						                              direction.y - 0.5f);
						newvec1.Normalize();
						BulletManager.FireBullet(transform.position, newvec1, Damage, true);
						goto case 2;
					}
						break;
					case 4:
					{
						Vector2 newvec = new Vector2(direction.x + 0.2f,
						                             direction.y + 0.2f);
						newvec.Normalize();
						BulletManager.FireBullet(transform.position, newvec, Damage, true);
						
						Vector2 newvec1 = new Vector2(direction.x - 0.2f,
						                              direction.y - 0.2f);
						newvec1.Normalize();
						BulletManager.FireBullet(transform.position, newvec1, Damage, true);

						Vector2 newvec2 = new Vector2(direction.x + 0.1f,
						                             direction.y + 0.1f);
						newvec2.Normalize();
						BulletManager.FireBullet(transform.position, newvec2, Damage, true);
						
						Vector2 newvec3 = new Vector2(direction.x - 0.1f,
						                              direction.y - 0.1f);
						newvec3.Normalize();
						BulletManager.FireBullet(transform.position, newvec3, Damage, true);

						goto case 3;
					}
						break;
					}
				}
				break;
			}

		}
		else
		{
			m_RigidBody.velocity = Vector2.zero;
		}

	}
}

