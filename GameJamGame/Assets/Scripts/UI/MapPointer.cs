using UnityEngine;
using System.Collections;

public class MapPointer : MonoBehaviour
{
	public void UpdatePointer(Vector2 Direction)
	{
		float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}

