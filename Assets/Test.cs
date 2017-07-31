using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(test);
    }

    private static void test()
    {
        var length = 1;
        LC.Online.Online.BuySlot(length, d =>
        {
            Debug.Log("Test.BuySlotCallback: " + (bool) d["status"]);
//            Debug.Log(d["slot"]);
        });
    }
}