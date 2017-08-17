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
        Map.Curr = Map.LoadFromOnline((string) dict["map"]);
        SceneManager.LoadScene("Play");
    }
}