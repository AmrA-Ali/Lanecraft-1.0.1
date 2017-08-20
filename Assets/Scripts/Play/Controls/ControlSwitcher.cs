using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class ControlSwitcher : MonoBehaviour {

    public GameObject m_car;


    public GameObject PlayModeButtons;
    public static float AcceloMode = 0,stopAxis=1;
    void Awake()
    {
        if (AcceloMode== 1)
        {
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(false);
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(false);
        }
        else
        {
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(true);
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(true);
        }
    }
    public void AcceloOn()
    {   AcceloMode = 1;
        stopAxis = 0;
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(false);
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(false);
    }

    public void ButtonsOn()
    {
        AcceloMode = 0;
        stopAxis = 1;
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(true);
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(true);
    }
}
