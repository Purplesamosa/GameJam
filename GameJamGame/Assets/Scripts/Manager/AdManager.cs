using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour {

	private static AdManager instance;

	public static AdManager Instance
	{
		get
		{
			return instance;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			DestroyImmediate(this);
		}
		Advertisement.Initialize("1018802");
	}
	
	public void ShowAd()
	{
		Advertisement.Show();
	}
}
