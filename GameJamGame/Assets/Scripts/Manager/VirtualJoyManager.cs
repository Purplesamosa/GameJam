using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoyManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

	public Transform m_JoyStick;

	public float m_Distance;
	private bool m_TrackMovement;

	void Start()
	{
		float _scalar = Screen.width/414.0f;
		m_Distance *= _scalar;
	}

	// Update is called once per frame
	void Update () 
	{
		if(m_TrackMovement)
		{
			if(Vector2.Distance(transform.position,Input.mousePosition) < m_Distance)
			{
				m_JoyStick.position = Input.mousePosition;
			}
			else
			{
				m_JoyStick.localPosition = (Vector3.Normalize(Input.mousePosition - transform.position)*m_Distance);
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		m_TrackMovement = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_TrackMovement = false;
		m_JoyStick.localPosition = Vector3.zero;
	}
}
