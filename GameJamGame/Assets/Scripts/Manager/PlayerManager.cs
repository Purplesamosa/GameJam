using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public FireBallManager m_FireBallManager;
	private bool m_CooledDownSpell = true;
	public float m_CoolDownTimer = 0.25f;
	public float m_CircleCastRadius = 5.0f;

	//changing velocites
	public float m_Speed;

	private float m_XVelo;
	private float m_YVelo;

	//reference to animator
	private Animator m_Animator;

	//reference to rigidbody
	private Rigidbody2D m_RigidBody;

	//health
	private bool m_IsDead = false;
	private int m_Level = 1;
	private float m_MaxHealth = 10.0f;
	private float m_Health = 10.0f;
	private float m_Damage = 1.0f;
	// Use this for initialization
	void Start () 
	{	
		m_Animator = GetComponent<Animator>();
		m_RigidBody = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		#region movement
	 	float x = m_Animator.GetFloat("XVelocity");
	 	float y = m_Animator.GetFloat("YVelocity");
		int _IdleState = m_Animator.GetInteger("IdleState"); //idle state
		Vector3 _hold = GameplayUIManager.Instance.GetJoyVelocities();
		m_YVelo = _hold.y;
		m_XVelo = _hold.x;
		if(Mathf.Abs(x) > Mathf.Abs(y) && !(x==0 && y==0))
		{
			if(x > 0)
			{
				_IdleState = 0;
			}
			else
			{
				_IdleState = 1;
			}
		}
		else if(!(x==0 && y==0))
		{
			if(y > 0)
			{
				_IdleState = 2;
			}
			else
			{
				_IdleState = 3;
			}
		}
		//3 = look down/ 2 = look up/ 1 = look left/ 0 = look right
		m_RigidBody.velocity = new Vector2(Time.deltaTime*m_XVelo*m_Speed,Time.deltaTime*m_YVelo*m_Speed);
		m_Animator.SetFloat("YVelocity",m_YVelo);
		m_Animator.SetFloat("XVelocity",m_XVelo);
		m_Animator.SetInteger("IdleState",_IdleState);
		#endregion
		#region Attack
		if(GameplayUIManager.Instance.GetFireButton() && m_CooledDownSpell)
		{
			m_CooledDownSpell = false;
			int _closest = 0;
			Collider2D[] _Hit2D = Physics2D.OverlapCircleAll(transform.position,m_CircleCastRadius,1<<9);
			if(_Hit2D.Length > 0)
			{
				for(int i = 0; i < _Hit2D.Length; ++i)
				{
					if(i != 0)
					{
						if(Vector2.Distance(transform.position,_Hit2D[i].transform.position)
						   < Vector2.Distance(transform.position,_Hit2D[_closest].transform.position))
						{
							_closest = i;
						}
					}
				}
			}
			switch(_IdleState)
			{
				case 0:
				{
					m_Animator.Play("PlayerCastRight");
					break;
				}
				case 1:
				{
					m_Animator.Play("PlayerCastLeft");
					break;
				}
				case 2:
				{
					m_Animator.Play("PlayerCastUp");
					break;
				}
				case 3:
				{
					m_Animator.Play("PlayerCastDown");
					break;
				}
			}
			if(_Hit2D.Length > 0)
			{
				if(_Hit2D[_closest])
				{
					Debug.Log("HERE?");
					m_FireBallManager.ShootFireball(_IdleState,_Hit2D[_closest].GetComponent<Enemy>());
				}
			}
			else
			{
				m_FireBallManager.ShootFireball(_IdleState);
			}
			StartCoroutine(CoolDownAttack());
		}
		#endregion
	}

	private IEnumerator CoolDownAttack()
	{
		yield return new WaitForSeconds(m_CoolDownTimer);
		m_CooledDownSpell = true;
	}

	public void TakeDamage(float Damage)
	{
		m_Health -= Damage;
	}

	private void HealthCheck()
	{
		if(m_Health <= 0)
		{
			m_IsDead = true;
		}
	}
	/*
	void FixedUpdate()
	{
		m_RigidBody.AddForce(new Vector2(m_XVelo*Time.fixedDeltaTime*m_Speed,m_YVelo*Time.fixedDeltaTime*m_Speed));
	}*/
}
