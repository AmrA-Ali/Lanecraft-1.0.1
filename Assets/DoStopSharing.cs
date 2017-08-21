using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene("Main");
        });
    }
}