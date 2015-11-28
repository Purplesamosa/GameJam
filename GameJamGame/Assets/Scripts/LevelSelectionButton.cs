using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelectionButton : MonoBehaviour
{
	public int World;
	public int Level;

	void OnEnable()
	{
		if(World == 1 && Level == 1)
		{
			GetComponent<Button>().interactable = true;
			return;
		}

		if(PlayerPrefs.GetInt("World" + World + "Level" + Level, 0) > 0)
		{
			GetComponent<Button>().interactable = true;
		}
		else
		{
			GetComponent<Button>().interactable = false;
		}
	}

	public void GoToLevel()
	{
		LevelBuilder.WorldToLoad = World;
		LevelBuilder.LevelToLoad = Level;
		Application.LoadLevel("MainScene");
	}
}

