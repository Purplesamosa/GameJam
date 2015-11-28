using UnityEngine;
using System.Collections;

public class EnemyBulletManager : MonoBehaviour
{
	public GameObject BulletPrefab;
	public Sprite FireballSprite;
	public Sprite HeartSprite;
	public int PoolSize = 100;
	public float BulletSpeed = 80.0f;
	private GameObject [] BulletPool;

	void Start()
	{
		BulletPool = new GameObject[PoolSize];
		for(int i = 0; i < BulletPool.Length; i++)
		{
			BulletPool[i] = Instantiate(BulletPrefab);
			BulletPool[i].SetActive(false);
		}
	}

	public void DeactivateAllBullets()
	{
		if(BulletPool == null)
			return;

		for(int i = 0; i < BulletPool.Length; i++)
		{
			if(BulletPool[i])
				BulletPool[i].SetActive(false);
		}
	}

	public void FireBullet(Vector2 Origin, Vector2 Direction, float Damage, bool IsBoss)
	{
		GameObject myBullet = null;

		for(int i = 0; i < BulletPool.Length; i++)
		{
			if(!BulletPool[i].activeSelf)
			{
				myBullet = BulletPool[i];
				break;
			}
		}

		if(!myBullet)
			return;

		myBullet.GetComponent<Animator>().enabled = true;
		myBullet.GetComponent<SpriteRenderer>().sprite = FireballSprite;

		float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
		myBullet.transform.position = Origin;
		myBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		switch(GetComponent<LevelBuilder>().World)
		{
		case 1:
			myBullet.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
			break;
		case 2:
			myBullet.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.5f, 1.0f);
			break;
		case 3:
			myBullet.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
			break;
		case 4:
			if(!IsBoss)
			{
				myBullet.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
				myBullet.GetComponent<SpriteRenderer>().sprite = HeartSprite;
				myBullet.GetComponent<Animator>().enabled = false;
				myBullet.transform.rotation = Quaternion.identity;
			}
			else
			{
				myBullet.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.1f, 0.9f);
			}
			break;
		}
		myBullet.GetComponent<EnemyBullet>().SetVars(BulletSpeed, Direction, Damage);
		myBullet.SetActive(true);
	}
}

