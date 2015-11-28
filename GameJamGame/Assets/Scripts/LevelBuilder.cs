using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour 
{
	public int World = 1;
	public int Level = 1;

	public Transform LevelContainer;

	void OnEnable()
	{
		CreateLevel(World,Level);
	}

	public void CreateLevel(int _world, int _level)
	{
		foreach(Transform tr in LevelContainer)
		{
			Destroy(tr.gameObject);
		}

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
		int MaxColumn = 0;
		Vector2 playerpos = new Vector2();
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
						tile.transform.parent = LevelContainer;
					}
					break;
				case '1':
					{
						GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Tiles/Tile" + (Random.Range(0,11)).ToString())) as GameObject;
						tile.transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						tile.transform.parent = LevelContainer;
					}
					break;
				case '2':
					{
						//Spawn player
						GameObject.FindObjectOfType<PlayerManager>().transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						playerpos = new Vector2(CurColumn * 1.59f, -i * 1.59f);
					goto case '1';
					}
				case '3':
					{
						//Spawn portal
						GameObject teleporter = Instantiate(Resources.Load("Prefabs/Teleporter")) as GameObject;
						teleporter.transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						teleporter.transform.parent = LevelContainer;
					goto case '1';
					}
				case '4':
					{
						//Spawn boss
						GameObject boss = Instantiate(Resources.Load("Prefabs/Enemies/World" + _world + "/Boss")) as GameObject;
						boss.transform.position = new Vector3(CurColumn * 1.59f, -i * 1.59f);
						boss.transform.parent = LevelContainer;
					goto case '1';
					}
				default:
					CurColumn--;
					break;
				}
				CurColumn++;
				if(MaxColumn < CurColumn)
				{
					MaxColumn = CurColumn;
				}
			}
		}

		//Spawn enemies randomly, a bit away from the player
		int numenemies = (_world + 2) * 5 + _level;

		List<Vector2> usedpositions = new List<Vector2>();

		for(int i = 0; i < numenemies; i++)
		{
			//Get a random row and column, if it's available (i.e. it's a 1), then spawn an enemy there.
			//Make sure it's at least 5 x and 5 y away from the player
			bool FoundPosition = false;
			while(!FoundPosition)
			{
				int randomrow = Random.Range(0, lines.Length-2);
				int randomcolumn = Random.Range(0, MaxColumn);

				Vector2 tempvec = new Vector2(randomcolumn * 1.59f, -randomrow * 1.59f);

				if(lines[randomrow][randomcolumn] == '1')
				{
					//Check that no enemy has spawned there
					bool bEmptyFlag = true;
					foreach(Vector2 vec in usedpositions)
					{
						if(vec.x == randomcolumn && vec.y == randomrow)
						{
							bEmptyFlag = false;
							break;
						}
					}

					if(bEmptyFlag)
					{
						//Check the distance from the player
						if(Vector2.SqrMagnitude( tempvec - playerpos ) > 25.0f)
						{
							FoundPosition = true;
							usedpositions.Add(new Vector2(randomcolumn, randomrow));

							//Now spawn randomly a ranged or melee
							if((randomrow & 1) == 0)
							{
								GameObject enemy = Instantiate(Resources.Load("Prefabs/Enemies/World" + _world + "/Ranged")) as GameObject;
								enemy.transform.position = new Vector3(randomcolumn * 1.59f, -randomrow * 1.59f);
								enemy.transform.parent = LevelContainer;
							}
							else
							{
								GameObject enemy = Instantiate(Resources.Load("Prefabs/Enemies/World" + _world + "/Melee")) as GameObject;
								enemy.transform.position = new Vector3(randomcolumn * 1.59f, -randomrow * 1.59f);
								enemy.transform.parent = LevelContainer;
							}
						}
					}
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
