using UnityEngine;
using UnityEngine.UI;
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

	private AudioSource m_AudioSource;

	//reference to renderer
	private SpriteRenderer m_SpriteRender;

	//reference to rigidbody
	private Rigidbody2D m_RigidBody;

	//health
	private bool m_IsDead = false;
	private bool m_FirstDeath = true;
	private int m_Level = 1;
	private float m_MaxHealth = 10.0f;
	private float m_Health = 10.0f;
	private float m_Damage = 1.0f;

	private int m_ExpForLevel;
	private int m_Exp;
	private int m_ExpToLevel;

	public Text m_CurrentLevel;

	public GameObject m_LevelUpText;

	private bool m_IsInvincible;
	// Use this for initialization
	void Start () 
	{	
		Debug.Log("PLAYER MANAGER CALLED START");
		m_Animator = GetComponent<Animator>();
		m_RigidBody = GetComponent<Rigidbody2D>();
		m_SpriteRender = GetComponent<SpriteRenderer>();
		m_AudioSource = GetComponent<AudioSource>();
		m_Level = PlayerPrefs.GetInt("Level",1);
		m_Health = m_MaxHealth = 10 * (3*m_Level);
		m_Damage = 1 + (m_Level);
		m_ExpToLevel = (int)(10 * (Mathf.Pow(4,m_Level)));
		m_Exp = PlayerPrefs.GetInt("EXP",0);
		m_CurrentLevel.text = "Level " + m_Level.ToString();
	}

	public void ReloadStats()
	{
		Debug.Log("ENABLED");
		m_Level = PlayerPrefs.GetInt("Level",1);
		m_Health = m_MaxHealth = 10 * (3*m_Level);
		m_Damage = 1 + (m_Level);
		m_ExpToLevel = (int)(10 * (Mathf.Pow(4,m_Level)));
		m_Exp = PlayerPrefs.GetInt("EXP",0);
		m_FirstDeath = true;
		GetComponent<SkillsManager>().ResetSkills();
		m_FireBallManager.DeactivateFireballs();
		m_CurrentLevel.text = "Level " + m_Level.ToString();
		GameplayUIManager.Instance.m_HealthBar.value = m_Health/m_MaxHealth*100;
		GameplayUIManager.Instance.m_ExpText.text = "Exp Earnt: " + 0;
	}

	// Update is called once per frame
	void Update () 
	{
		#region Pause
		if(GameplayUIManager.Instance.GetPauseButton())
		{
			GameplayUIManager.Instance.SetUpThePause();
			Time.timeScale = 0;
		}
		#endregion
		if(!m_IsDead)
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
						m_FireBallManager.ShootFireball(_IdleState,m_Damage,_Hit2D[_closest].GetComponent<Enemy>());
					}
				}
				else
				{
					m_FireBallManager.ShootFireball(_IdleState,m_Damage);
				}
				m_AudioSource.Play();
				StartCoroutine(CoolDownAttack());
			}
			#endregion
		}
	}

	private IEnumerator CoolDownAttack()
	{
		yield return new WaitForSeconds(m_CoolDownTimer);
		m_CooledDownSpell = true;
	}

	public void TakeDamage(float Damage)
	{
		if(!m_IsInvincible)
		{
			m_IsInvincible = true;
			StartCoroutine(UnInvincible());
			m_Health -= Damage;
			GameplayUIManager.Instance.m_HealthBar.value = m_Health/m_MaxHealth*100;
			HealthCheck();
		}
	}

	private IEnumerator UnInvincible()
	{
		m_SpriteRender.color = Color.red;
		yield return new WaitForSeconds(0.2f);
		if(m_Health > 0)
		{
			m_IsInvincible = false;
		}
		m_SpriteRender.color = Color.white;
	}

	private void HealthCheck()
	{
		if(m_Health <= 0)
		{
			m_Animator.Play("PlayerDie");
			m_IsDead = true;
			m_IsInvincible = true;
			if(m_FirstDeath)
			{
				m_FirstDeath = false;
				StartCoroutine(LoadUpSaveLife());
			}
			else
			{
				GameplayUIManager.Instance.FadeToMenu();
			}
		}
	}

	public void ChoosedToLive()
	{
		Time.timeScale = 1;
		AdManager.Instance.ShowAd();
		m_Animator.Play ("PlayerResurrected");
		m_Health = m_MaxHealth;
		GameplayUIManager.Instance.m_HealthBar.value = m_Health/m_MaxHealth*100;
		m_IsDead = false;
		m_IsInvincible = false;
	}

	public void ChoosedToDie()
	{
		Time.timeScale = 1;
		GameplayUIManager.Instance.FadeToMenu();
	}

	private IEnumerator LoadUpSaveLife()
	{
		yield return new WaitForSeconds(1.5f);
		Time.timeScale = 0;
		GameplayUIManager.Instance.m_SaveLifePanel.SetActive(true);
	}

	public void GiveXP(int xp)
	{
		m_ExpForLevel += xp;
	}

	public void AwardXP(bool DoubleXP = false)
	{
		if(DoubleXP)
		{
			AdManager.Instance.ShowAd();
		}
		StartCoroutine(AddExp(DoubleXP));
	}

	private IEnumerator AddExp(bool DoubleXP)
	{
		int _total = m_ExpForLevel;
		int _Text = 0;
		if(DoubleXP)
		{
			_Text = _total;
		}
		else
		{
			_Text = 0;
		}
		GameplayUIManager.Instance.m_ExpBar.value = (float)m_Exp/(float)m_ExpToLevel;
		while(_total > 0)
		{
			if(_total > 10)
			{
				_total -= 10;
				_Text += 10;
			}
			else
			{
				_Text += _total;
				_total = 0;
			}
			GameplayUIManager.Instance.m_ExpText.text = "Exp Earnt: " + _Text.ToString();
			Debug.Log(m_ExpToLevel/(_Text+m_Exp));
			GameplayUIManager.Instance.m_ExpBar.value = (float)(_Text+m_Exp)/(float)m_ExpToLevel;
			yield return 0;
		}
		if(m_ExpToLevel < (m_Exp+_Text))
		{
			++m_Level;
			m_LevelUpText.SetActive(true);
			PlayerPrefs.SetInt("Level",m_Level);
			_Text -= (m_ExpToLevel-m_Exp);
			m_Exp = _Text;
			PlayerPrefs.SetInt("EXP",m_Exp);
			ReloadStats();
			Debug.Log(m_Exp.ToString());
			GameplayUIManager.Instance.m_ExpBar.value = (float)m_Exp/(float)m_ExpToLevel;
		}
	}

	public IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}


	public int GetEXP()
	{
		return m_Exp;
	}

	public int GetLevelXP()
	{
		return m_ExpToLevel;
	}

	public string GetHP()
	{
		return m_Health.ToString() + "/" + m_MaxHealth.ToString();
	}

	public float GetDamage()
	{
		return m_Damage;
	}

	/*
	void FixedUpdate()
	{
		m_RigidBody.AddForce(new Vector2(m_XVelo*Time.fixedDeltaTime*m_Speed,m_YVelo*Time.fixedDeltaTime*m_Speed));
	}*/
}
