using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FetchMapList : MonoBehaviour
{
    [SerializeField]
    private Button MapButton;
    public static string PlaySceneName = "_Play";

    void Awake()
    {
        Map[] ListofMaps = Map.FetchMapsInfo();
        Button gb;
        for (int i = 0; i < ListofMaps.Length; i++)
        {
            gb = Instantiate(MapButton);
            string name = ListofMaps[i].info.name; 
            gb.GetComponentInChildren<Text>().text = name;
            gb.name = PlaySceneName;
            gb.GetComponent<SelectedMapSetter>().selectedMap = ListofMaps[i];
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
