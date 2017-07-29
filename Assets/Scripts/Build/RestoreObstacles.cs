using UnityEngine;

public class RestoreObstacles : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        Map.curr.ActivateObs();
	}
    void Start()
    {
        Map.curr.ActivateObs();
    }
	
}
