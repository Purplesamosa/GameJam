using UnityEngine;
using System.Collections;

public class BarrageProjectile : MonoBehaviour
{
	private float DeactivationTimer = 0.0f;
	public Vector2 Direction;
	private float Speed = 200.0f;
	private float Damage = 5.0f;
	private float AttackTimer = 0.0f;

	private bool bAttack = false;
	
	public void SetDir(Vector2 _dir)
	{
		Direction = _dir;
		Direction.Normalize();
	}
	
	void OnEnable()
	{
		Damage = GameplayUIManager.Instance.m_Player.GetDamage() * 1.5f;

		bAttack = false;
		DeactivationTimer = 0.0f;
		AttackTimer = 0.0f;
		GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.x * Speed * Time.deltaTime, Direction.y * Speed * Time.deltaTime);
		float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		Enemy pm = col.gameObject.GetComponent<Enemy>();
		if(pm)
		{
			pm.TakeDamage(Damage);
			bAttack = false;
			AttackTimer = 0.0f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		DeactivationTimer += Time.deltaTime;
		AttackTimer += Time.deltaTime;

		if(AttackTimer >= 0.1f && !bAttack)
		{
			bAttack = true;
			//Find the closest enemy and hit him
			Collider2D[] _Hit2D = Physics2D.OverlapCircleAll(transform.position,5.0f,1<<9);
			int _closest = 0;
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

				Direction = (_Hit2D[_closest].transform.position - transform.position).normalized;

				GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.x * Speed * Time.deltaTime, Direction.y * Speed * Time.deltaTime);
				float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				
			}
		}

		if(DeactivationTimer >= 3.0f)
		{
			gameObject.SetActive(false);
		}
	}
}

