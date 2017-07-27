using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QPPressed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
        GetComponent<Button>().onClick.AddListener(delegate { DoQP(); });
	}
	void DoQP(){
		
		Online.QP(QPDone);
	}
	void QPDone(){
		Debug.Log("CallBack Recieved!");
	}
}
