using UnityEngine;
using UnityEngine.UI;

public class DoDelete : MonoBehaviour
{
	void Start ()
	{
		GetComponent<Button> ().onClick.AddListener (delegate {
			Confirm ();
		});
	}

	void Confirm ()
	{
		print(Map.Curr.Delete ());
	}

}
