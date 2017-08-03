using UnityEngine;
using System;

public class FetchMyMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject _mapButton;

	void Start ()
	{
		Map[] listofMaps = Offline.Maps;
		listofMaps = Array.FindAll(listofMaps, m1 => m1.Info.Creator.Equals(Player.Data.Creator()));//Filtering out all offline maps
		GameObject gb;
		for (int i = 0; i < listofMaps.Length; i++) {
			gb = Instantiate (_mapButton);
			foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {
				j.DisplayMapButton( listofMaps [i]);
			}
			gb.transform.SetParent (transform);
			gb.transform.localScale = new Vector3 (1, 1, 1);
		}
	}	
}