using UnityEngine;
using UnityEngine.UI;
public class DoneSave : MonoBehaviour {
    [SerializeField]
    private Text mapName;
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { Confirm(); });
	}
    void Confirm()  {
        BuildSession.map.info.name = mapName.text;
        BuildSession.map.Save();
        BuildSession.map = new global::Map();
    }
}
