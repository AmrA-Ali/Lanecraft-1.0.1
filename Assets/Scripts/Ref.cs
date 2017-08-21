using UnityEngine;

//Used as a reference to the monobehaviour for any static method to use
public class Ref : MonoBehaviour {
	public static Ref Inst;
	void Awake(){
		Inst = this;
	}
}
