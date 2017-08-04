using UnityEngine;
using UnityEngine.UI;

public class DisplayFinalScore : MonoBehaviour {

	// Use this for initialization
    int CalculateScore()
    {
        return 10000 / (int)Stopwatch.time;
    }
    void Start() {
        GetComponent<Text>().text = "Time: " + Stopwatch.time + "\n" + "Score: " + CalculateScore() +"\n";
	}
}
