using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FetchOfflineMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject MapButton;

	void Start ()
	{
		Map[] ListofMaps = Offline.maps;
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
