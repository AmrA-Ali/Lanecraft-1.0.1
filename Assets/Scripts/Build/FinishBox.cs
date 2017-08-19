using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishBox : MonoBehaviour {

	// Use this for initialization

	void Start () {
        if (gameObject.name.Contains("Build"))
            GetComponent<Button>().onClick.AddListener(delegate { Map.curr.RemoveLastBrick(); });
        else GetComponent<Button>().onClick.AddListener(delegate { Map.curr.AddFinishLine(); });
    }
}
