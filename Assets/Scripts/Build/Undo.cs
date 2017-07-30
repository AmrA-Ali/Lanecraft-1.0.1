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
        if (gameObject.name.Contains("Brick"))
            Map.curr.RemoveLastBrick();
        else Map.curr.RemoveLastObstacle();
    }
}
