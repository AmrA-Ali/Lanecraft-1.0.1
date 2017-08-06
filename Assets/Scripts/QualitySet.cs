using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QualitySet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(QualitySettings.GetQualityLevel());
        //QualitySettings.SetQualityLevel(5);
        print(QualitySettings.GetQualityLevel());
    }
	
}
