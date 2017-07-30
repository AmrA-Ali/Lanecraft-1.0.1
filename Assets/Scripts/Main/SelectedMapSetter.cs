using UnityEngine;
using UnityEngine.UI;
public class SelectedMapSetter : MonoBehaviour
{
	[SerializeField]
	private Map selectedMap;

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { setMap(); }); }
	
    //set the map in the Datatransfer script.
	void setMap()
	{
		gameObject.setMap(selectedMap);
	}
	public void SetSelectedMap(Map map){
		selectedMap = map;
		gameObject.name = "_MapInfo";
		GameObject rating = transform.GetChild(0).gameObject;
		GameObject title = transform.GetChild(1).gameObject;
		GameObject creator = transform.GetChild(2).gameObject;
		GameObject uploadDate = transform.GetChild(3).gameObject;
		GameObject playCount = transform.GetChild(4).gameObject;
		creator.GetComponentInChildren<Text>().text = map.info.creator;
		title.GetComponentInChildren<Text>().text = map.info.name;

	}
}