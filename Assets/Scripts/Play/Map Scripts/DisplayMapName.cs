using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DisplayMapName : MonoBehaviour {
	void Start () 
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Build") 
            GetComponent<Text>().text = gameObject.map().info.name;
        else GetComponent<Text>().text = BuildSession.map.info.name;
    }
}
