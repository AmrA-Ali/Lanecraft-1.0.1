using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
public class Car : MonoBehaviour {
    public static MastarCarController mCC;
    public static MastarCarUserControl mCUC;
    public static PullerScript pS;

    void Start () {
        mCC = GetComponent<MastarCarController>();
        mCUC =  GetComponent<MastarCarUserControl>();
        pS =  GetComponent<PullerScript>();

        SetActiveCarScripts(false);
    }
    public static void SetActiveCarScripts(bool active)
    {
        mCC.enabled = active;
        mCUC.enabled = active;
        pS.enabled = active;
    }
}
