using UnityEngine;
using System.Collections;

public class BuildSession : MonoBehaviour {

    // Use this for initialization
    public static Map map;
	void Start () {
        map = new global::Map();
    }
}
