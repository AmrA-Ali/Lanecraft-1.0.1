using UnityEngine;
using System.Collections;
using LC.Online;
public class FetchOnlineMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject _mapButton;
	// public GameObject Loading;
	
	void Start ()
	{
		if(!Online.MapsReady){
			Online.GetMaps();
		}
		StartCoroutine(DisplayMaps());
	}

	IEnumerator DisplayMaps(){

		while(!Online.MapsReady){
			yield return null;
		}
		
		Map[] listofMaps = Online.Maps;
		GameObject gb;
		for (int i = 0; i < listofMaps.Length; i++) {
			gb = Instantiate (_mapButton);
			foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {
				j.DisplayMapButton( listofMaps [i]);
			}
			gb.transform.SetParent (transform);
			gb.transform.localScale = new Vector3 (1, 1, 1);
		}
		// Loading.SetActive(false);
		yield break;
	}
}