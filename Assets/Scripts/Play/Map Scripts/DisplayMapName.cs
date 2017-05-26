using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DisplayMapName : MonoBehaviour {
	void Start () 
    {
        GetComponent<Text>().text = gameObject.map().info.name;
    }
}
