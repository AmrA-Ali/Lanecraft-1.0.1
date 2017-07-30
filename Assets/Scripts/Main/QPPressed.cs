using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LC.Economy;

public class QPPressed : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DoQp);
    }

    private void DoQp()
    {
        if (EconomyManager.CanQuickPlay())
        {
            Online.QP(QpDone);
        }
    }

    private void QpDone(Dictionary<string, object> dict)
    {
        var map = Map.CollectionToMap(dict);
        gameObject.setMap(map);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}