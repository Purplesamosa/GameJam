using UnityEngine;
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
}
