using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	public float m_Speed;

	private Rigidbody2D m_RigidBody;

	private bool m_IgnoreTarget = true;

	public Enemy m_Target;

	// Use this for initialization
	void Awake () 
	{
		if(!m_RigidBody)
		{
			m_RigidBody = GetComponent<Rigidbody2D>();
		}
	}

	void OnDisable()
	{
		m_IgnoreTarget = true;
		transform.localPosition = Vector3.zero;
		m_RigidBody.velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_IgnoreTarget && m_Target)
		{
			if(m_Target.gameObject.activeSelf)
			{
				Vector2 _Direction = m_Target.transform.position - transform.position ;
				_Direction.Normalize();
				Debug.Log(_Direction);
				float _Angle = Mathf.Atan2(_Direction.y,_Direction.x)*Mathf.Rad2Deg;
				_Angle += 90;
				transform.rotation = Quaternion.AngleAxis(_Angle,Vector3.forward);
				m_RigidBody.velocity = _Direction*m_Speed*Time.deltaTime;
			}
		}
	}

	public void Shoot(int initalDir, Enemy target = null)
	{
		gameObject.SetActive(true);
		m_Target = target;
		switch(initalDir)
		{
			case 3: 
			{
				transform.localEulerAngles = new Vector3(0,0,0);
				m_RigidBody.velocity = new Vector2(0,-m_Speed*Time.deltaTime);
				break;
			}
			case 2: 
			{
				transform.localEulerAngles = new Vector3(0,0,180);
				m_RigidBody.velocity = new Vector2(0,m_Speed*Time.deltaTime);
				break;
			}
			case 1: 
			{
				transform.localEulerAngles = new Vector3(0,0,270);
				m_RigidBody.velocity = new Vector2(-m_Speed*Time.deltaTime,0);
				break;
			}
			case 0: 
			{
				transform.localEulerAngles = new Vector3(0,0,90);
				m_RigidBody.velocity = new Vector2(m_Speed*Time.deltaTime,0);
				break;
			}
		}
		StartCoroutine(AwakenBullet());
	}

	private IEnumerator AwakenBullet()
	{
		yield return new WaitForSeconds(0.5f);
		m_IgnoreTarget = false;
		yield return new WaitForSeconds(5.0f);
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		Debug.Log(other.name);
		if(other.CompareTag("Enemies"))
		{
			gameObject.SetActive(false);
		}
	}
}
