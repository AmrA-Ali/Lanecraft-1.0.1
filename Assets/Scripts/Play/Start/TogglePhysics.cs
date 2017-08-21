using UnityEngine;
using UnityEngine.UI;

public class TogglePhysics : MonoBehaviour { 
        void Start () { GetComponent<Button>().onClick.AddListener(delegate { ToggleTimeScale(); }); }
	// Update is called once per frame
	void ToggleTimeScale()
    {Time.timeScale = Time.timeScale == 0 ? 1 : 0;}
}
