using UnityEngine;
using System.Collections;

public class DisplayFinalScore : MonoBehaviour {

	// Use this for initialization
    int CalculateScore()
    {
        return 10000 / (int)Stopwatch.time;
    }
    void Start() {
        GetComponent<UnityEngine.UI.Text>().text = "Time: " + Stopwatch.time.ToString() + "\n" + "Score: " + CalculateScore().ToString() +"\n";
	}
    void OnEnable()
    {
        GetComponent<UnityEngine.UI.Text>().text = "Time: " + Stopwatch.time.ToString() + "\n" + "Score: " + CalculateScore().ToString() + "\n";   
    }
}
