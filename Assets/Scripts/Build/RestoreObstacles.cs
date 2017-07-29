using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreObstacles : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        RestoreObs();
	}
    public void RestoreObs()
    {
        var allObs = GameObject.FindGameObjectsWithTag("obstacle");
        foreach (var e in allObs)
        {
            e.SetActive(true);
            print(e.name);
        }
    }
	
}
