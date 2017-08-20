using UnityEngine;
public class Stopwatch : MonoBehaviour {
    public static float time = 0;
    public static void Set()
    {
        time = 0;
    }
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
       
	}
}
