using UnityEngine;
using System.Collections;
using LC.Online;
public class FetchOnlineMaps : MonoBehaviour
{
	[SerializeField]
	private GameObject MapButton;
	// public GameObject Loading;
	
	void Start ()
	{
		if(!Online.mapsReady){
			Online.GetMaps();
		}
		StartCoroutine(DisplayMaps());
	}

	IEnumerator DisplayMaps(){

		while(!Online.mapsReady){
			yield return null;
		}
		
		Map[] ListofMaps = Online.maps;
		GameObject gb;
		for (int i = 0; i < ListofMaps.Length; i++) {
			gb = Instantiate (MapButton);
			foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {
				j.DisplayMapButton( ListofMaps [i]);
			}
			gb.transform.SetParent (transform);
			gb.transform.localScale = new Vector3 (1, 1, 1);
		}
		// Loading.SetActive(false);
		yield break;
	}
}