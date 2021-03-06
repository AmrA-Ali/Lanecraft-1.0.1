﻿using LC.Economy;
using LC.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Initialization Starting...");
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
                        //update the Economy
                        EconomyManager.GetReady(() =>
                        {
                            //Now everything is ready
                            Debug.Log("Initialization Done!");
                            SceneManager.LoadScene("Main");
                        });
                    });
                });
            });
        });
    }
}