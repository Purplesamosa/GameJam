using UnityEngine;
using System.Collections;

public class FireBallManager : MonoBehaviour {

	public Fireball[] m_Fireballs;

	public void DeactivateFireballs()
	{
		for(int i = 0; i < m_Fireballs.Length; ++i)
		{
			m_Fireballs[i].gameObject.SetActive(false);
		}
	}

	public void ShootFireball(int initalDir,float damage , Enemy target = null)
	{
		for(int i = 0; i < m_Fireballs.Length; ++i)
		{
			if(!m_Fireballs[i].gameObject.activeSelf)
			{
				//m_Fireballs[i].gameObject.SetActive(true);
				m_Fireballs[i].Shoot(initalDir, damage,target);
				break;
			}
		}
	}
}
