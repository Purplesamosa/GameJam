using UnityEngine;
using System.Collections;

public class PopUpAnimation : MonoBehaviour {

	void OnEnable()
	{
		transform.localScale = Vector3.one*0.1f;
	}

	void Update()
	{
		if(transform.localScale.x < 1)
		{
			transform.localScale += Vector3.one*0.1f;
		}
	}
}
