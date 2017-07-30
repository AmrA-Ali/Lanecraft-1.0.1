using UnityEngine;
using System.Collections;
using LC.SaveLoad;
using LC.Online;
public class Initializer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		StartCoroutine(Init());
	}
	
	public IEnumerator Init(){
		Debug.Log("Initializer");
		Time.timeScale = 1;
		gameObject.setMap(new Map());
		while(!SaveLoadManager.READY){
			yield return null;
		}
		
		while(!Player.READY){
			yield return null;
		}


		Offline.GetMaps();

		if (Player.ONLINE){
			Online.GetMaps();//Fetch the online maps list and update it in @Online
		}

	}
}
