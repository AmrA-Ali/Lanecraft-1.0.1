using System.Collections.Generic;
using LC.Economy;
using LC.Online;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QpPressed : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoQp);
    }

    private void DoQp()
    {
        if (EconomyManager.CanQuickPlay())
        {
            Loading.StartLoading();
            Online.Qp(QpDone);
        }
    }

    private void QpDone(Dictionary<string, object> dict)
    {
        Loading.StopLoading();
        if (dict == null) return;
        var map = Map.LoadFromOnline((string) dict["map"]);
        gameObject.SetMap(map);
        SceneManager.LoadScene("Play");
    }
}