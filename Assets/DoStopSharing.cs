using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoStopSharing : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    void TheAction()
    {
        Loading.StartLoading();
        Slot.Remove(Map.curr, cb =>
        {
            Loading.StopLoading();
        });
    }
}