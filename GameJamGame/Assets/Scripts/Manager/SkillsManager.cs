using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillsManager : MonoBehaviour
{
	public Button Skill1;
	public Button Skill2;
	public Button Skill3;

	public float MaxCooldown = 5.0f;

	public static bool bHasSkill1 = false;
	public static bool bHasSkill2 = false;
	public static bool bHasSkill3 = false;

	public GameObject NovaProjectilePrefab;

	private float SkillCooldown1 = 0.0f;
	private float SkillCooldown2 = 0.0f;
	private float SkillCooldown3 = 0.0f;

	private NovaProjectile [] NovaProjectilesPool;

	void OnEnable()
	{
		//CheckSkills();
	}

	void Start()
	{
		NovaProjectilesPool = new NovaProjectile[8];
		for(int i = 0; i < NovaProjectilesPool.Length; i++)
		{
			GameObject temp = Instantiate(NovaProjectilePrefab);
			NovaProjectilesPool[i] = temp.GetComponent<NovaProjectile>();
			temp.SetActive(false);
		}

	}

	public void CheckSkills()
	{
		if(PlayerPrefs.GetInt("World2Level1", 0) > 0)
		{
			Skill1.gameObject.SetActive(true);
			bHasSkill1 = true;
		}
		if(PlayerPrefs.GetInt("World3Level1", 0) > 0)
		{
			Skill2.gameObject.SetActive(true);
			bHasSkill2 = true;
		}
		if(PlayerPrefs.GetInt("World4Level1", 0) > 0)
		{
			Skill3.gameObject.SetActive(true);
			bHasSkill3 = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		SkillCooldown1 = Mathf.Max(0.0f, SkillCooldown1 - Time.deltaTime);
		SkillCooldown2 = Mathf.Max(0.0f, SkillCooldown2 - Time.deltaTime);
		SkillCooldown3 = Mathf.Max(0.0f, SkillCooldown3 - Time.deltaTime);

		/*if(SkillCooldown1 <= 0.0f)
		{
			Skill1.interactable = true;
		}
		if(SkillCooldown2 <= 0.0f)
		{
			Skill2.interactable = true;
		}
		if(SkillCooldown3 <= 0.0f)
		{
			Skill3.interactable = true;
		}*/

		if(bHasSkill1 && Input.GetKeyDown(KeyCode.Alpha1) && SkillCooldown1 <= 0.0f)
		{
			//Skill1.interactable = false;
			//Dark fire nova
			NovaProjectilesPool[0].SetDir(new Vector2(-1.0f, 0.0f));
			NovaProjectilesPool[1].SetDir(new Vector2(1.0f, 0.0f));
			NovaProjectilesPool[2].SetDir(new Vector2(0.0f, 1.0f));
			NovaProjectilesPool[3].SetDir(new Vector2(0.0f, -1.0f));
			NovaProjectilesPool[4].SetDir(new Vector2(-0.6f, 0.6f));
			NovaProjectilesPool[5].SetDir(new Vector2(0.6f, 0.6f));
			NovaProjectilesPool[6].SetDir(new Vector2(0.6f, -0.6f));
			NovaProjectilesPool[7].SetDir(new Vector2(-0.6f, -0.6f));

			for(int i = 0; i < NovaProjectilesPool.Length; i++)
			{
				NovaProjectilesPool[i].gameObject.SetActive(true);
				NovaProjectilesPool[i].transform.position = transform.position + new Vector3(NovaProjectilesPool[i].Direction.x,
				                                                                             NovaProjectilesPool[i].Direction.y,
				                                                                             0.0f) * 0.3f;
			}
		}

		if(bHasSkill2 && Input.GetKeyDown(KeyCode.Alpha2) && SkillCooldown2 <= 0.0f)
		{
			Skill2.interactable = false;
			//Magic barrage
		}

		if(bHasSkill3 && Input.GetKeyDown(KeyCode.Alpha3) && SkillCooldown3 <= 0.0f)
		{
			Skill3.interactable = false;
			//Lightning
		}

	}
}

