using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	//changing velocites
	public float m_Speed;

	private float m_XVelo;
	private float m_YVelo;

	//reference to animator
	private Animator m_Animator;

	//reference to rigidbody
	private Rigidbody2D m_RigidBody;

	// Use this for initialization
	void Start () 
	{	
		m_Animator = GetComponent<Animator>();
		m_RigidBody = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () 
	{
	 	float x = m_Animator.GetFloat("XVelocity");
	 	float y = m_Animator.GetFloat("YVelocity");
		int _IdleState = m_Animator.GetInteger("IdleState"); //idle state
		/*if(Input.GetKey(KeyCode.A))
		{
			m_XVelo = -1;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			m_XVelo = 1;
		}
		else
		{
			m_XVelo = 0;
		}
		if(Input.GetKey(KeyCode.W))
		{
			m_YVelo = 1;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			m_YVelo = -1;
		}
		else 
		{
			m_YVelo = 0;
		}*/
		Vector3 _hold = GameplayUIManager.Instance.GetJoyVelocities();
		m_YVelo = _hold.y;
		m_XVelo = _hold.x;
		Debug.Log ("IDLE STATE : " + _IdleState);
		if(Mathf.Abs(x) > Mathf.Abs(y) && !(x==0 && y==0))
		{
			if(x > 0)
			{
				Debug.Log("Idle 0");
				_IdleState = 0;
			}
			else
			{
				Debug.Log("Idle 1");
				_IdleState = 1;
			}
		}
		else if(!(x==0 && y==0))
		{
			if(y > 0)
			{
				Debug.Log("Idle 2");
				_IdleState = 2;
			}
			else
			{
				Debug.Log("Idle 3");
				_IdleState = 3;
			}
		}
		//3 = look down/ 2 = look up/ 1 = look left/ 0 = look right
		m_RigidBody.velocity = new Vector2(Time.deltaTime*m_XVelo*m_Speed,Time.deltaTime*m_YVelo*m_Speed);
		m_Animator.SetFloat("YVelocity",m_YVelo);
		m_Animator.SetFloat("XVelocity",m_XVelo);
		m_Animator.SetInteger("IdleState",_IdleState);
	}
	/*
	void FixedUpdate()
	{
		m_RigidBody.AddForce(new Vector2(m_XVelo*Time.fixedDeltaTime*m_Speed,m_YVelo*Time.fixedDeltaTime*m_Speed));
	}*/
}
