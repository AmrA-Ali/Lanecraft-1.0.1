using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;

public class LogOut : MonoBehaviour {

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { Log_out(); }); }
	
	void Log_out(){
		GS.Reset();

			UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
	}
}
