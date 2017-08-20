using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour {

    Text text;
	void Start () {
        text = GetComponent<Text>();
	}
    void OnEnable()
    {
        Stopwatch.Set();
    }
	
	// Update is called once per frame
	void Update () {
        text.text =  Stopwatch.time.ToString();
	}
}
