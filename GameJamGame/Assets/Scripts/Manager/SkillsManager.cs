using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillsManager : MonoBehaviour
{
	public Button Skill1;
	public Button Skill2;
	public Button Skill3;

	public float MaxCooldown = 5.0f;

	public static bool bHasSkill1 = true;
	public static bool bHasSkill2 = true;
	public static bool bHasSkill3 = true;

	public GameObject NovaProjectilePrefab;
	public GameObject BarragePrefab;
	public GameObject MeteorPrefab;

	private float SkillCooldown1 = 0.0f;
	private float SkillCooldown2 = 0.0f;
	private float SkillCooldown3 = 0.0f;

	private Vector2 [] Dirs;

	private NovaProjectile [] NovaProjectilesPool;
	private BarrageProjectile [] BarrageProjectilesPool;
	private Meteor MyMeteor;

	private int CurBarrage = 0;

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

		BarrageProjectilesPool = new BarrageProjectile[8];
		for(int i = 0; i < BarrageProjectilesPool.Length; i++)
		{
			GameObject temp = Instantiate(BarragePrefab);
			BarrageProjectilesPool[i] = temp.GetComponent<BarrageProjectile>();
			temp.SetActive(false);
		}

		GameObject tempobj = Instantiate(MeteorPrefab);
		MyMeteor = tempobj.GetComponent<Meteor>();
		tempobj.SetActive(false);

		Dirs = new Vector2[8];

		Dirs[0] = new Vector2(-1.0f, 0.0f);
		Dirs[1] = new Vector2(1.0f, 0.0f);
		Dirs[2] = new Vector2(0.0f, 1.0f);
		Dirs[3] = new Vector2(0.0f, -1.0f);
		Dirs[4] = new Vector2(-0.6f, 0.6f);
		Dirs[5] = new Vector2(0.6f, 0.6f);
		Dirs[6] = new Vector2(0.6f, -0.6f);
		Dirs[7] = new Vector2(-0.6f, -0.6f);



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
			SkillCooldown1 = 5.0f;

			for(int i = 0; i < NovaProjectilesPool.Length; i++)
			{
				NovaProjectilesPool[i].SetDir(Dirs[i]);
				NovaProjectilesPool[i].gameObject.SetActive(true);
				NovaProjectilesPool[i].transform.position = transform.position + new Vector3(NovaProjectilesPool[i].Direction.x,
				                                                                             NovaProjectilesPool[i].Direction.y,
				                                                                             0.0f) * 0.3f;
			}
		}

		if(bHasSkill2 && Input.GetKeyDown(KeyCode.Alpha2) && SkillCooldown2 <= 0.0f)
		{
			SkillCooldown2 = 5.0f;
			//Skill2.interactable = false;
			//Magic barrage
			CurBarrage = 0;
			InvokeRepeating("FireBarrage", 0.01f, 0.1f);
		}

		if(/*bHasSkill3 &&*/ Input.GetKeyDown(KeyCode.Alpha3) && SkillCooldown3 <= 0.0f)
		{
			SkillCooldown3 = 5.0f;
			//Skill3.interactable = false;
			//Lightning
			MyMeteor.gameObject.SetActive(true);
			MyMeteor.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 1.5f, transform.position.z);
		}

	}

	void FireBarrage()
	{
		BarrageProjectilesPool[CurBarrage].transform.position = transform.position;
		BarrageProjectilesPool[CurBarrage].SetDir(Dirs[Random.Range(0, Dirs.Length)]);
		BarrageProjectilesPool[CurBarrage].gameObject.SetActive(true);
		CurBarrage++;
		if(CurBarrage >= BarrageProjectilesPool.Length)
		{
			CancelInvoke("FireBarrage");
			return;
		}
	}
}

