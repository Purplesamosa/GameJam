using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoyManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{

	public Transform m_JoyStick;
	
	public float m_Distance;
	private bool m_TrackMovement;
	private int m_TouchIndex;

	float _scalar;

	void Start()
	{
		_scalar = Screen.width/414.0f;
		m_Distance *= _scalar;
	}

	// Update is called once per frame
	void Update () 
	{
		if(m_TrackMovement)
		{
#if UNITY_EDITOR
			if(Vector2.Distance(transform.position,Input.mousePosition) < m_Distance)
			{
				m_JoyStick.localPosition = (Input.mousePosition-transform.position)/_scalar;
			}
			else
			{
				m_JoyStick.localPosition = (Vector3.Normalize(Input.mousePosition - transform.position)*m_Distance)/_scalar;
			}
#else
			if(Vector2.Distance(transform.position, Input.GetTouch(m_TouchIndex).position) < m_Distance)
			{
				Vector3 _MyVec = new Vector3(Input.GetTouch(m_TouchIndex).position.x,
				                             Input.GetTouch(m_TouchIndex).position.y,0);
				m_JoyStick.localPosition = (_MyVec-transform.position)/_scalar;
			}
			else
			{
				Vector3 _MyVec = new Vector3(Input.GetTouch(m_TouchIndex).position.x,
				                             Input.GetTouch(m_TouchIndex).position.y,0);
				m_JoyStick.localPosition = (Vector3.Normalize(_MyVec - transform.position)*m_Distance)/_scalar;
			}

#endif
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		m_TouchIndex = eventData.pointerId;
		m_TrackMovement = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_TrackMovement = false;
		m_JoyStick.localPosition = Vector3.zero;
	}
}
