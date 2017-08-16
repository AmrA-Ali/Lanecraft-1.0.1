using LC.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Initializer");
        Loading.StartLoading();
        Time.timeScale = 1;
        Map.Default();
        //wait for the SaveLoadManager to get ready
        SaveLoadManager.GetReady(() =>
        {
            //Wait for the Player to get ready
            Player.GetReady(() =>
            {
                //Prepare the maps from offline and online and fuse them
                Map.GetReady(() =>
                {
                    //update the slots
                    Slot.UpdateSlotsFromOnline(() =>
                    {
                        //Now everything is ready
                        Debug.Log("Ready");
                        SceneManager.LoadScene("Main");
                    });
                });
            });
        });
    }
}