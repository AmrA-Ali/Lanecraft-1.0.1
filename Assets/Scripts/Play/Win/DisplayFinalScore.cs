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
    void OnEnable()
    {
        GetComponent<UnityEngine.UI.Text>().text = "Time: " + Stopwatch.time.ToString() + "\n" + "Score: " + CalculateScore().ToString() + "\n";   
    }
}
