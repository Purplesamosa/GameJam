using UnityEngine;
using System.Collections;

public class FireBallManager : MonoBehaviour {

	public Fireball[] m_Fireballs;

	public void ShootFireball(int initalDir, Enemy target = null)
	{
		for(int i = 0; i < m_Fireballs.Length; ++i)
		{
			if(!m_Fireballs[i].gameObject.activeSelf)
			{
				//m_Fireballs[i].gameObject.SetActive(true);
				m_Fireballs[i].Shoot(initalDir,target);
				break;
			}
		}
	}
}
