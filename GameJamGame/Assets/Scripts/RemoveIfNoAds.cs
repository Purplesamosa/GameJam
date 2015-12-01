﻿using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class RemoveIfNoAds : MonoBehaviour {

	void OnEnable()
	{
		if(!Advertisement.IsReady("rewardedVideo"))
		{
			gameObject.SetActive(false);
		}
	}
}
