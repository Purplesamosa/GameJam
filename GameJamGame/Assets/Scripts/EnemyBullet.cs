using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
	private float DeactivationTimer = 0.0f;
	public Vector2 Direction;
	public float Speed;

	// Use this for initialization
	void OnEnable()
	{
		DeactivationTimer = 0.0f;
		GetComponent<Rigidbody2D>().velocity = new Vector2(Direction.x * Speed * Time.deltaTime, Direction.y * Speed * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log("VELOCITY: " + GetComponent<Rigidbody2D>().velocity);
		DeactivationTimer += Time.deltaTime;
		if(DeactivationTimer >= 3.0f)
		{
			gameObject.SetActive(false);
		}
	}
}

