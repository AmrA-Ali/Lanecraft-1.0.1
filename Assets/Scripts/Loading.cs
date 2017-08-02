using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public static GameObject LoadingText;
    public static GameObject inst;

    public static void StartLoading()
    {
        if (!LoadingText)
            LoadingText = Resources.Load<GameObject>("Prefabs/UI/LoadingText");
        if (inst) return;
        var canvas = GameObject.Find("Canvas");
        inst = Instantiate(LoadingText, canvas.transform);
        inst.transform.localScale = new Vector3(1, 1, 1);
    }

    public static void StopLoading()
    {
        if (!inst) return;
        Destroy(inst);
    }
}