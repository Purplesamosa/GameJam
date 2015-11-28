using UnityEngine;
using System.Collections;

public class CreditsMover : MonoBehaviour
{
	public float StartingY;
	public float EndingY;
	public float Speed;

	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime, transform.position.z);

		if(GetComponent<RectTransform>().localPosition.y >= EndingY)
		{
			GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, 
			                                                          StartingY, 
			                                                          GetComponent<RectTransform>().localPosition.z);
		}
	}
}

