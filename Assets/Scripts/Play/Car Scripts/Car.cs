﻿using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Car : MonoBehaviour {
    public static MastarCarController mCC;
    public static MastarCarUserControl mCUC;
    public static PullerScript pS;
    void Start () {
        mCC = GetComponent<MastarCarController>();
        mCUC =  GetComponent<MastarCarUserControl>();
        pS =  GetComponent<PullerScript>();
        //SetActiveCarScripts(false);
    }
    void OnEnable()
    {
        Return();
    }
    public static void SetActiveCarScripts(bool active)
    {
        mCC.enabled = active;
        mCUC.enabled = active;
        pS.enabled = active;
    }
    public void Return()
    {
        transform.position = new Vector3(0, 1.1f, -6.02f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
