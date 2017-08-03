using UnityEngine;

public class Loading : MonoBehaviour
{
    public static GameObject LoadingText;
    public static GameObject Inst;

    public static void StartLoading()
    {
        if (!LoadingText)
            LoadingText = Resources.Load<GameObject>("Prefabs/UI/LoadingText");
        if (Inst) return;
        var canvas = GameObject.Find("Canvas");
        Inst = Instantiate(LoadingText, canvas.transform);
        Inst.transform.localScale = new Vector3(1, 1, 1);
    }

    public static void StopLoading()
    {
        if (!Inst) return;
        Destroy(Inst);
    }
}