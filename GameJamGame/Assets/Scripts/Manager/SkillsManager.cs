using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillsManager : MonoBehaviour
{
	public Button Skill1;
	public Button Skill2;
	public Button Skill3;

	public Image Button1Img;
	public Image Button2Img;
	public Image Button3Img;

	public float MaxCooldown = 5.0f;

	public static bool bHasSkill1 = false;
	public static bool bHasSkill2 = false;
	public static bool bHasSkill3 = false;

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
		CheckSkills();
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

	public void ResetSkills()
	{
		SkillCooldown1 = SkillCooldown2 = SkillCooldown3 = 0.0f;

		for(int i = 0; i < NovaProjectilesPool.Length; i++)
		{
			NovaProjectilesPool[i].gameObject.SetActive(false);
		}

		for(int i = 0; i < BarrageProjectilesPool.Length; i++)
		{
			BarrageProjectilesPool[i].gameObject.SetActive(false);
		}
		
		MyMeteor.gameObject.SetActive(false);
	}

	public void CheckSkills()
	{
		Skill1.gameObject.SetActive(false);
		Skill2.gameObject.SetActive(false);
		Skill3.gameObject.SetActive(false);

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

		if(SkillCooldown1 <= 0.0f)
		{
			Skill1.interactable = true;
			Button1Img.color = new Color(1.0f, 1.0f, 1.0f);
		}
		if(SkillCooldown2 <= 0.0f)
		{
			Skill2.interactable = true;
			Button2Img.color = new Color(1.0f, 1.0f, 1.0f);
		}
		if(SkillCooldown3 <= 0.0f)
		{
			Skill3.interactable = true;
			Button3Img.color = new Color(1.0f, 1.0f, 1.0f);
		}

		
		bool bKeySkill1 = false;
		bool bKeySkill2 = false;
		bool bKeySkill3 = false;
		
		#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.X))
		{
			bKeySkill1 = true;
		}
		
		if(Input.GetKey(KeyCode.C))
		{
			bKeySkill2 = true;
		}
		
		if(Input.GetKey(KeyCode.V))
		{
			bKeySkill3 = true;
		}
		#endif

		if(bHasSkill1 && (Skill1.GetComponent<UIButton>().m_Status || bKeySkill1) && SkillCooldown1 <= 0.0f)
		{
			Skill1.interactable = false;
			//Dark fire nova
			SkillCooldown1 = 5.0f;
			Button1Img.color = new Color(0.5f, 0.5f, 0.5f);
			
			for(int i = 0; i < NovaProjectilesPool.Length; i++)
			{
				NovaProjectilesPool[i].SetDir(Dirs[i]);
				NovaProjectilesPool[i].gameObject.SetActive(true);
				NovaProjectilesPool[i].transform.position = transform.position + new Vector3(NovaProjectilesPool[i].Direction.x,
				                                                                             NovaProjectilesPool[i].Direction.y,
				                                                                             0.0f) * 0.3f;
			}
		}


		if(bHasSkill2 && (Skill2.GetComponent<UIButton>().m_Status || bKeySkill2) && SkillCooldown2 <= 0.0f)
		{
			SkillCooldown2 = 5.0f;
			Skill2.interactable = false;
			Button2Img.color = new Color(0.5f, 0.5f, 0.5f);
			//Magic barrage
			CurBarrage = 0;
			InvokeRepeating("FireBarrage", 0.01f, 0.1f);
		}

		if(bHasSkill3 && (Skill3.GetComponent<UIButton>().m_Status || bKeySkill3)  && SkillCooldown3 <= 0.0f)
		{
			SkillCooldown3 = 5.0f;
			Skill3.interactable = false;
			Button3Img.color = new Color(0.5f, 0.5f, 0.5f);
			//Meteor
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

