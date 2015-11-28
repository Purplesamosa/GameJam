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
	
	// Update is called once per frame
	void Update () {
	
	}
}
