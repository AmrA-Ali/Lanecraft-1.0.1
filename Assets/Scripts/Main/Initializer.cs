using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Time.timeScale = 1;
		gameObject.setMap(new Map());
		LoadMaps();
	}
	
	void LoadMaps(){
		if (Player.ONLINE)
		Online.GetMaps();//Fetch the online maps list and update it in @Online

		Offline.GetMaps();
	}
}
