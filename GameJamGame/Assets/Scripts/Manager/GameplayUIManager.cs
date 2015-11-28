﻿using UnityEngine;
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

	public Text m_ExpText;
	public Slider m_ExpBar;

	public Slider m_HealthBar;
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
		return m_FireButton.m_Status;
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
}
