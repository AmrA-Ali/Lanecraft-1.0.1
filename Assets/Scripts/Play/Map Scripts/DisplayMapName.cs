using UnityEngine;
using UnityEngine.UI;
public class DisplayMapName : MonoBehaviour {
	void Start () 
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Build") 
            GetComponent<Text>().text = gameObject.Map().Info.Name;
        else GetComponent<Text>().text = Map.Curr.Info.Name;
    }
}
