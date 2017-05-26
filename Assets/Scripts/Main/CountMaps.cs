using UnityEngine;
//Check if there are any available maps
public class CountMaps : MonoBehaviour {
    public static bool ThereAreMaps=false;
	void Start () { ThereAreMaps = (SaveLoadManager.FetchMapsInfoCodes().Length >0); }
}