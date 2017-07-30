using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoDownload : MonoBehaviour
{
	void Start ()
	{
		GetComponent<Button> ().onClick.AddListener (delegate {
			Map.curr.Download();
			});
	}	
}
