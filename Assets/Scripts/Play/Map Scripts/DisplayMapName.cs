using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayMapName : MonoBehaviour {
	void Start () 
    {
        if (SceneManager.GetActiveScene().name != "Build") 
            GetComponent<Text>().text = gameObject.Map().Info.Name;
        else GetComponent<Text>().text = Map.Curr.Info.Name;
    }
}
