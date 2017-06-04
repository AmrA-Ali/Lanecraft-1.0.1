using UnityEngine;
using System.Collections;

public class DetectWin : MonoBehaviour {

    private GameObject boxcoll;
    // Use this for initialization
    void Start()
    {
    // boxcoll = GameObject.FindGameObjectWithTag("Finish");
    }
	void OnTriggerEnter()
    {
        boxcoll = GameObject.FindGameObjectWithTag("Finish");
        boxcoll.SwitchPage("page");
    }
}
