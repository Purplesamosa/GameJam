using UnityEngine;
using System.Collections;

public class CreditsMover : MonoBehaviour
{
	public float StartingY;
	public float EndingY;
	public float Speed;
	public bool bStop = false;

	void OnEnable()
	{
		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime, transform.position.z);

		if(GetComponent<RectTransform>().localPosition.y >= EndingY)
		{

			if(bStop)
			{
				Speed = 0.0f;
				return;
			}

			GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, 
			                                                          StartingY, 
			                                                          GetComponent<RectTransform>().localPosition.z);
		}
	}
}

