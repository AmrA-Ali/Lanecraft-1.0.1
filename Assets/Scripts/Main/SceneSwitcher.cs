using UnityEngine;
using UnityEngine.UI;
public class SceneSwitcher : MonoBehaviour {
	void Start () { GetComponent<Button>().onClick.AddListener(delegate { gameObject.SwitchScene(); }); }
}