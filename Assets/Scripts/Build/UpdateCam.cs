using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (gameObject.name.Contains("Obs"))
            GetComponent<Button>().onClick.AddListener(delegate { Map.curr.CamToLastObs(); });
        else
            GetComponent<Button>().onClick.AddListener(delegate { Map.curr.CamToLastBrick(); });
    }
	
}
