using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 using UnityEngine.UI;
 
public class DisplaySelector : MonoBehaviour {

 public Dropdown myDropdown;
 public Transform pages;
	// Use this for initialization
	void Start () {
		myDropdown.onValueChanged.AddListener(delegate {
         myDropdownValueChangedHandler(myDropdown);
     });
	}
	
 private void myDropdownValueChangedHandler(Dropdown target) {
 	for(int i=0;i<pages.childCount;i++){
 		pages.GetChild(i).gameObject.SetActive(target.value==i);
 	}
     Debug.Log("selected: "+target.value);
 	
 }
	// Update is called once per frame
	void Update () {
		
	}
	public void Select(int i){

	}
}
