using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
	private float DeactivationTimer = 0.0f;
	public Vector2 Direction;
	public float Speed;
	public float Damage;

	// Use this for initialization
	void OnEnable()
	{
		DeactivationTimer = 0.0f;
		GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.x * Speed * Time.deltaTime, Direction.y * Speed * Time.deltaTime);
	}

	public void SetVars(float _speed, Vector3 _dir, float _dmg)
	{
		Speed = _speed;
		Direction = _dir;
		Damage = _dmg;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		PlayerManager pm = col.gameObject.GetComponent<PlayerManager>();
		if(pm)
		{
			pm.TakeDamage(Damage);
			gameObject.SetActive(false);
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

