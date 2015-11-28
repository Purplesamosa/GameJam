using UnityEngine;
using System.Collections;

public class EnemyFadeOut : MonoBehaviour
{
	public Sprite DeathSprite;

	private SpriteRenderer sr;
	private float alpha = 1.0f;

	void OnEnable ()
	{
		sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sprite = DeathSprite;
	}
	
	// Update is called once per frame
	void Update ()
	{
		alpha -= Time.deltaTime;
		alpha = Mathf.Max(alpha, 0.0f);
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

		if(alpha <= 0.0f)
		{
			gameObject.SetActive(false);
		}
	}
}

