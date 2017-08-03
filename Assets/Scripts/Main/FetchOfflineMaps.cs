using UnityEngine;

public class FetchOfflineMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject _mapButton;

	void Start ()
	{
		Map[] listofMaps = Offline.Maps;
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
