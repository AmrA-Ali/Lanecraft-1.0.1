using GameSparks.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogOut : MonoBehaviour {

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { Log_out(); }); }
	
	void Log_out(){
		GS.Reset();

			SceneManager.LoadScene("Login");
	}
}
