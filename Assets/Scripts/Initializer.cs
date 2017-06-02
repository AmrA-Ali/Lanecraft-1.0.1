using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Time.timeScale = 1;
        gameObject.setMap(new Map());
	}
}
