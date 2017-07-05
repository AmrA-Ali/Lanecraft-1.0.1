using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FetchMapList : MonoBehaviour
{
	[SerializeField]
	private GameObject MapButton;
	public static string PlaySceneName = "_Play";

	void Start ()
	{
		// if(!Offline.mapsReady){
		// 	Offline.GetMaps();
		// }
		Map[] ListofMaps = Offline.maps;
		GameObject gb;
		for (int i = 0; i < ListofMaps.Length; i++) {
			gb = Instantiate (MapButton);
			string MapName = ListofMaps [i].info.name; 
			gb.GetComponentInChildren<Text> ().text = MapName;
			gb.name = PlaySceneName;//Not needed with ModifiedMapSelectButton as it's hardcoded in the children
			foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>()) {//The loop is needed for the ModifiedMapSelecButton
				j.selectedMap = ListofMaps [i];
			}
			gb.transform.SetParent (transform);
			gb.transform.localScale = new Vector3 (1, 1, 1);
		}
	}
}
