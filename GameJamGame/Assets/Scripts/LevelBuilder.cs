using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour 
{
	public int World = 1;
	public int Level = 1;

	public static int WorldToLoad = 1;
	public static int LevelToLoad = 1;

	public Transform LevelContainer;

	public GameObject LevelCompletedScreen;

	public MapPointer MyMapPointer;

	public PlayerManager m_Player;

	private Teleporter MyTeleporter;

	private Transform TargetObj;

	public MusicManager m_MusicManager;

	public GameObject m_LoadingPanel;

	private float Separation = 1.65f;

	public Sprite [] WorldBG;
	public SpriteRenderer MyBGImage;

	void OnEnable()
	{

		CreateLevel(WorldToLoad, LevelToLoad);
	}

	void Update()
	{
		if(!TargetObj || !m_Player)
			return;

		Vector2 TargetPos = TargetObj.position;
		Vector2 CurPos = m_Player.transform.position;
		MyMapPointer.UpdatePointer( (TargetPos - CurPos).normalized );

#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(Time.timeScale == 0.0f)
			{
				Time.timeScale = 1.0f;
			}
			else
			{
				Time.timeScale = 0.0f;
			}
		}
#endif
	}

	public void GoToNextLevel()
	{
		StartCoroutine(LoadNextLevel());
	}

	private IEnumerator LoadNextLevel()
	{
		m_LoadingPanel.SetActive(true);
		yield return null;
		LevelToLoad++;
		bool bFinishedGame = false;
		if(LevelToLoad > 5)
		{
			WorldToLoad++;
			m_MusicManager.ChangeSong();
			LevelToLoad = 1;
			if(WorldToLoad >= 5)
			{
				//WE BEAT THE GAME!!!
				bFinishedGame = true;
				Time.timeScale = 1.0f;
				Application.LoadLevel("EndScene");
				yield return null;
			}
		}

		if(!bFinishedGame)
		{
			m_Player.ReloadStats();
		
			CreateLevel(WorldToLoad, LevelToLoad);
		}
	}

	public void FinishLevel()
	{
		bool bUnlock = true;
		Level++;
		if(Level > 5)
		{
			World++;
			Level = 1;
			if(World > 5)
			{
				//We beat the game
				bUnlock = false;
			}
		}
		if(bUnlock)
		{
			PlayerPrefs.SetInt("World" + World + "Level" + Level, 1);
		}

		LevelCompletedScreen.SetActive(true);
		LevelCompletedScreen.transform.GetChild(0).gameObject.SetActive(false);
		m_Player.AwardXP();
		Time.timeScale = 0.0f;
	}

	public void CreateLevel(int _world, int _level)
	{
		GetComponent<EnemyBulletManager>().DeactivateAllBullets();

		MyBGImage.sprite = WorldBG[_world - 1];

		Time.timeScale = 1.0f;
		World = WorldToLoad;
		Level = LevelToLoad;
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
						tile.transform.position = new Vector3(CurColumn * Separation, -i * Separation);
						tile.transform.parent = LevelContainer;
					}
					break;
				case '1':
					{
						GameObject tile = Instantiate(Resources.Load("Prefabs/" + Season + "/Tiles/Tile" + (Random.Range(0,11)).ToString())) as GameObject;
						tile.transform.position = new Vector3(CurColumn * Separation, -i * Separation);
						tile.transform.parent = LevelContainer;
					}
					break;
				case '2':
					{
						//Spawn player
						GameObject.FindObjectOfType<PlayerManager>().transform.position = new Vector3(CurColumn * Separation, -i * Separation);
						playerpos = new Vector2(CurColumn * Separation, -i * Separation);
					goto case '1';
					}
				case '3':
					{
						//Spawn portal
						GameObject teleporter = Instantiate(Resources.Load("Prefabs/Teleporter")) as GameObject;
						teleporter.transform.position = new Vector3(CurColumn * Separation, -i * Separation);
						teleporter.transform.parent = LevelContainer;
						MyTeleporter = teleporter.GetComponent<Teleporter>();
						MyTeleporter.MyLevelBuilder = this;
						TargetObj = teleporter.transform;
					goto case '1';
					}
				case '4':
					{
						//Spawn boss
						GameObject boss = Instantiate(Resources.Load("Prefabs/Enemies/World" + _world + "/Boss")) as GameObject;
						boss.transform.position = new Vector3(CurColumn * Separation, -i * Separation);
						boss.transform.parent = LevelContainer;
						boss.GetComponent<Enemy>().MyLevelBuilder = this;
						TargetObj = boss.transform;
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
		numenemies *= 3;

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

				Vector2 tempvec = new Vector2(randomcolumn * Separation, -randomrow * Separation);

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
								enemy.transform.position = new Vector3(randomcolumn * Separation, -randomrow * Separation);
								enemy.transform.parent = LevelContainer;
							}
							else
							{
								GameObject enemy = Instantiate(Resources.Load("Prefabs/Enemies/World" + _world + "/Melee")) as GameObject;
								enemy.transform.position = new Vector3(randomcolumn * Separation, -randomrow * Separation);
								enemy.transform.parent = LevelContainer;
							}
						}
					}
				}
			}
		}
		m_LoadingPanel.SetActive(false);

	}
}
