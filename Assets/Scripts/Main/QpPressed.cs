using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LC.Economy;
using LC.Online;

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
            Online.Qp(QpDone);
        }
    }

    private void QpDone(Dictionary<string, object> dict)
    {
        var map = Map.CollectionToMap(dict);
        gameObject.SetMap(map);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}