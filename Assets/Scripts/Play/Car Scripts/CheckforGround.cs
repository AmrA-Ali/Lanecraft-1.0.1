using UnityEngine;
using System.Collections;

public class CheckforGround : MonoBehaviour {

    public static int type = 0;
    public static Transform getMid;
    public static int neg = 1;
    private int conti = -1;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cube"))
        {
            getMid = other.gameObject.transform.parent;
            type = 1;
        }
        else if (other.name.Contains("CurveDown"))
        {
            getMid = other.gameObject.transform.parent;
            type = 2;
        }
        else type = 0;
        print(other.name);
        
       /* print(type);
        print(getMid.forward);
        print(getMid.eulerAngles.x);
        print("Rotation: " + getMid.rotation.x);*/
    }

    void OnTriggerExit(Collider other)
    {
        //type = 0;
        //print(type);
    }
}
