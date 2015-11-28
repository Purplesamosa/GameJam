using UnityEngine;
using System.Collections;

public class LoadSceneOnClick : MonoBehaviour
{

	public void LoadScene(string _sceneName)
	{
		Application.LoadLevel(_sceneName);
	}
}

