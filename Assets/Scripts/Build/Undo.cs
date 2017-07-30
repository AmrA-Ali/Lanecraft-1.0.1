using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Undo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(undo);
	}
    private static void undo()
    {
        Map.curr.RemoveLastObject();
    }
}
