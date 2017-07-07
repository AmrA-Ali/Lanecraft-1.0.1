using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DisplaySelector : MonoBehaviour {

	public Transform pages;
	// Use this for initialization
	void Start () {
		Dropdown dd = GetComponent<Dropdown>();
		dd.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(dd);
			});
	}
	
	private void myDropdownValueChangedHandler(Dropdown target) {
		for(int i=0;i<pages.childCount;i++){
			pages.GetChild(i).gameObject.SetActive(target.value==i);
		}
	}
}
