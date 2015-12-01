using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayUIManager : MonoBehaviour {

	//singleton!!!!!!$$£%£^^&$&!!£$%^&*()_
	public static GameplayUIManager Instance
	{
		get
		{
			return instance;
		}
	}
	private static GameplayUIManager instance;

	public VirtualJoyManager m_JoyManager;

	public GameObject m_SaveLifePanel;

	public Image m_FadeToMenu;
	public Text m_FadeToMenuText;

	public UIButton m_FireButton;
	public UIButton m_PauseButton;

	public Text m_ExpText;
	public Slider m_ExpBar;

	public Slider m_HealthBar;

	public PlayerManager m_Player;

	public GameObject m_PausePanel;
	public Text m_CurrentText;
	public Text m_ExpToLevelText;
	public Text m_HealthText;
	public Text m_DamageText;
	public Text m_LevelText;
	// Use this for initialization
	void Awake ()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	
	public Vector3 GetJoyVelocities()
	{
		return Vector3.Normalize(m_JoyManager.m_JoyStick.position-m_JoyManager.transform.position);
	}

	public bool GetFireButton()
	{
		bool bKeyFire = false;
#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.Z))
		{
			bKeyFire = true;
		}
		else
		{
			bKeyFire = false;
		}
#endif
		return (m_FireButton.m_Status || bKeyFire);
	}

	public void FadeToMenu()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		m_FadeToMenu.gameObject.SetActive(true);
		while(m_FadeToMenu.color.a < 1)
		{
			float A = m_FadeToMenu.color.a;
			m_FadeToMenu.color = new Color(m_FadeToMenu.color.r,m_FadeToMenu.color.g,m_FadeToMenu.color.b,A+0.05f);
			m_FadeToMenuText.color = new Color(m_FadeToMenuText.color.r,m_FadeToMenuText.color.g,m_FadeToMenuText.color.b,A+0.05f);
			yield return null;
		}
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel ("MenuScene");
	}

	public bool GetPauseButton()
	{
		return m_PauseButton.m_Status;
	}

	public void SetUpThePause()
	{
		m_PausePanel.SetActive(true);
		m_LevelText.text = "Level: " + m_Player.GetLevel();
		m_CurrentText.text = "EXP: " + m_Player.GetEXP();
		m_ExpToLevelText.text = "Till level: " + m_Player.GetLevelXP();
		m_HealthText.text = m_Player.GetHP();
		m_DamageText.text = m_Player.GetDamage().ToString();
	}

	public void Resume()
	{
		m_PausePanel.SetActive(false);
		Time.timeScale = 1;
	}
}
