using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour 
{
	public int World = 1;
	public int Level = 1;

	void OnEnable()
	{
		CreateLevel(World,Level);
	}

	public void CreateLevel(int _world, int _level)
	{
		TextAsset myTextAsset;

		string filename = "LevelFiles/World" + World.ToString() + "/" + Level.ToString();
		myTextAsset = Resources.Load(filename) as TextAsset;
		if(!myTextAsset)
			return;

		string[] lines = myTextAsset.text.Split("\n"[0]);
		string Season = "Spring";

		switch(_world)
		{
		case 2:
			Season = "Summer";
			break;
		case 3:
			Season = "Autumn";
			break;
		case 4:
			Season = "Winter";
			break;
		}
		int CurColumn = 0;
		for(int i = 0; i < lines.Length; i++)
		{
			CurColumn = 0;
			for(int character = 0; character < lines[i].Length; character++)
			{
				switch(lines[i][character])
				{
				case '0':
					{
						GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Obstacles/Obstacle")) as GameObject;
						tile.transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						tile.transform.parent = transform;
					}
					break;
				case '1':
					{
						GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Tiles/Tile" + (Random.Range(0,11)).ToString())) as GameObject;
						tile.transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						tile.transform.parent = transform;
					}
					break;
				case '2':
					{
						//Spawn player
					goto case '1';
					}
					break;
				case '3':
					{
						//Spawn portal
					goto case '1';
					}
					break;
				case '4':
					{
						//Spawn boss
					goto case '1';
					}
					break;
				default:
					CurColumn--;
					break;
				}
				CurColumn++;
			}
		}

		/*FileInfo theSourceFile = new FileInfo ("Assets/Resources/LevelFiles/World1/1.txt");
		StreamReader reader = theSourceFile.OpenText();
				
		string text = reader.ReadLine();
				
		while (text != null)
		{
			Debug.Log("Line " + text);
			text = reader.ReadLine();
		}*/       
	}
}
