using LC.Economy;
using UnityEngine;
using UnityEngine.UI;

public class DoneSave : MonoBehaviour
{
    [SerializeField] private InputField _mapName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Confirm);
    }

    private void Confirm()
    {
        if (!EconomyManager.CanSaveMap()) return;
        var mapName = _mapName.text;
        Map.Save(mapName);
//        var m = new Map
//        {
//            Info = {Name = _mapName.text},
//            Bricks = Map.Curr.Bricks
//        };
//        m.SaveOffline();
        //Map.curr = new global::Map();//This makes a confusion as the Build editor keeps the old map while building a new one
        Debug.Log("Calling Offline.GetMaps() after Saveing");
        Offline.GetMaps();
    }
}