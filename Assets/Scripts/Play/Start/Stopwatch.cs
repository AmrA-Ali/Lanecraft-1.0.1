using UnityEngine;

public class Stopwatch : MonoBehaviour {
    public static float time;
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
       
	}
}
