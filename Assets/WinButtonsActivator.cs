using System.Collections;
using System.Collections.Generic;
using LC.Economy;
using UnityEngine;

public class WinButtonsActivator : MonoBehaviour
{
    public GameObject DropDown;
    public GameObject Favorite;

    // Use this for initialization
    void Start()
    {
        if (EconomyManager.CanFavoriteMap())
        {
            DropDown.SetActive(true);
            Favorite.SetActive(true);
        }
        else
        {
            DropDown.SetActive(false);
            Favorite.SetActive(false);
        }
    }
}