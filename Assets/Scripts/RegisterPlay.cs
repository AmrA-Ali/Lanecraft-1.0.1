using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegisterPlay : MonoBehaviour {

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { Online.AddPlay(); }); }
	
}
