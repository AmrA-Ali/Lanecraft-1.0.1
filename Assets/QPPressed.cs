using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Responses;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QPPressed : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { DoQp(); });
    }

    void DoQp()
    {
        Online.QP(QpDone);
    }

    void QpDone(Dictionary<string, object> dict)
    {
        Debug.Log("CallBack Recieved!");
        var map = Map.CollectionToMap(dict);
        gameObject.setMap(map);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }
}