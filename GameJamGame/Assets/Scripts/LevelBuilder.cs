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

		for(int i = 0; i < lines.Length; i++)
		{
			for(int character = 0; character < lines[i].Length; character++)
			{
				if(lines[i][character] == '0')
				{
					GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Obstacles/Obstacle")) as GameObject;
					tile.transform.position = new Vector3(character * 1.59f, -i * 1.59f);
					tile.transform.parent = transform;
				}
				else
				{
					GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Tiles/Tile" + (Random.Range(0,11)).ToString())) as GameObject;
					tile.transform.position = new Vector3(character * 1.59f, -i * 1.59f);
					tile.transform.parent = transform;
				}
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
