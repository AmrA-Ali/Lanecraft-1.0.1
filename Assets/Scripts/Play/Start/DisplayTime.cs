using UnityEngine;

public class DisplayTime : MonoBehaviour {

    UnityEngine.UI.Text text;
	void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = Stopwatch.time.ToString();
	}
}
