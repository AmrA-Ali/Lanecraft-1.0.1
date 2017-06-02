using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Undo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { undo(); });
	}
    private void undo()
    {
        BuildSession.map.RemoveLastObject();
    }
}
