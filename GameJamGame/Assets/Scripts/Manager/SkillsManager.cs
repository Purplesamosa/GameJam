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

	private float SkillCooldown1 = 0.0f;
	private float SkillCooldown2 = 0.0f;
	private float SkillCooldown3 = 0.0f;

	void OnEnable()
	{
		CheckSkills();
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

		if(SkillCooldown1 <= 0.0f)
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
		}

		if(bHasSkill1 && Input.GetKeyDown(KeyCode.Alpha1) && SkillCooldown1 <= 0.0f)
		{
			Skill1.interactable = false;
			//Fire nova
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

