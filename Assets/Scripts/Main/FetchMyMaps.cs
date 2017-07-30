using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FetchMyMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject MapButton;

	void Start ()
	{
		Map[] ListofMaps = Offline.maps;
		ListofMaps = Array.FindAll(ListofMaps, m1 => m1.info.creator.Equals(Player.DATA.Creator()));//Filtering out all offline maps
		GameObject gb;
		for (int i = 0; i < ListofMaps.Length; i++) {
			gb = Instantiate (MapButton);
			foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {
				j.DisplayMapButton( ListofMaps [i]);
			}
			gb.transform.SetParent (transform);
			gb.transform.localScale = new Vector3 (1, 1, 1);
		}
	}	
}