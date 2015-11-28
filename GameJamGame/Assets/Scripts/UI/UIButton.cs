using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler{

	public bool m_Status;

	private bool m_FingerDown = false;

	public void OnPointerDown(PointerEventData eventData)
	{
		m_FingerDown = true;
		m_Status = true;
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		m_FingerDown = false;
		m_Status = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_Status = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if(m_FingerDown)
		{
			m_Status = true;
		}
	}
}
