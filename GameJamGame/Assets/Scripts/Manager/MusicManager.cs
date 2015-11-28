using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] m_AudioClip;

	public LevelBuilder m_LevelBuilder;


	// Use this for initialization
	void Start () 
	{
		GetComponent<AudioSource>().clip = m_AudioClip[m_LevelBuilder.World-1];
		GetComponent<AudioSource>().Play();
	}
	
	public void ChangeSong()
	{
		GetComponent<AudioSource>().clip = m_AudioClip[m_LevelBuilder.World-1];
		GetComponent<AudioSource>().Play();
	}
}
