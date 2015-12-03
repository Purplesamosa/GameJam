using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
	private float FallTimer = 0.0f;
	private float alpha = 1.0f;
	private SpriteRenderer sr;
	private bool bStopped = false;


	// Use this for initialization
	void OnEnable ()
	{
		alpha = 1.0f;
		FallTimer = 0.0f;
		bStopped = false;
		sr = GetComponent<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
		GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -100.0f * Time.deltaTime);
		GetComponent<Animator>().enabled = true;
		GetComponent<Collider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		FallTimer += Time.deltaTime;
		if(FallTimer >= 1.0f)
		{
			if(!bStopped)
			{
				bStopped = true;
				GetComponent<Collider2D>().enabled = true;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Animator>().enabled = false;
			}
			alpha -= Time.deltaTime;
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
			if(alpha <= 0.0f)
			{
				GetComponent<Collider2D>().enabled = false;
				gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Enemy en = col.GetComponent<Enemy>();
		if(en)
		{
			if(en.MyType == Enemy.EnemyType.Boss)
			{
				en.TakeDamage(GameplayUIManager.Instance.m_Player.GetDamage() * 3.0f);
			}
			else
			{
				en.TakeDamage(100.0f);
			}
		}
	}
}

