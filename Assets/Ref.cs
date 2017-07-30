using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used as a reference to the monobehaviour for any static method to use
public class Ref : MonoBehaviour {
	public static Ref INST;
	void Awake(){
		INST = this;
	}
}
