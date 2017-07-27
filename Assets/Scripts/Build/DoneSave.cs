using UnityEngine;
using UnityEngine.UI;
public class DoneSave : MonoBehaviour {
    [SerializeField]
	private InputField mapName;
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { Confirm(); });
	}
    void Confirm()  {
        Map.curr.info.name = mapName.text;
        Map.curr.Save();
        //Map.curr = new global::Map();//This makes a confusion as the Build editor keeps the old map while building a new one
        Offline.GetMaps();
        Debug.Log("Calling Offline.GetMaps() after Saveing");
    }
}
