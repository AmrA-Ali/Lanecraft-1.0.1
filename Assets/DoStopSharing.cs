using UnityEngine;
using UnityEngine.UI;

public class DoStopSharing : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    void TheAction()
    {
        Loading.StartLoading();
        Slot.Remove(Map.Curr, cb =>
        {
            Loading.StopLoading();
            //todo refresh the affected things, like maps
        });
    }
}