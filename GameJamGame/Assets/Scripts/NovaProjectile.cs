using UnityEngine;
using System.Collections;

public class NovaProjectile : MonoBehaviour
{
	private float DeactivationTimer = 0.0f;
	public Vector2 Direction;
	private float Speed = 100.0f;
	private float Damage = 5.0f;

	public void SetDir(Vector2 _dir)
	{
		Direction = _dir;
		Direction.Normalize();
	}

	void OnEnable()
	{

		Damage = GameplayUIManager.Instance.m_Player.GetDamage() + 2.0f;

		DeactivationTimer = 0.0f;
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
			//gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		DeactivationTimer += Time.deltaTime;
		if(DeactivationTimer >= 3.0f)
		{
			gameObject.SetActive(false);
		}
	}
}

