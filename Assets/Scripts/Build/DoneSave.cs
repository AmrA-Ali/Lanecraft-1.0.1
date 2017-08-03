using UnityEngine;
using UnityEngine.UI;
using LC.Economy;

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
        var m = new Map
        {
            Info = {Name = _mapName.text},
            Bricks = Map.Curr.Bricks
        };
        m.Save();
        //Map.curr = new global::Map();//This makes a confusion as the Build editor keeps the old map while building a new one
        Debug.Log("Calling Offline.GetMaps() after Saveing");
        Offline.GetMaps();
    }
}