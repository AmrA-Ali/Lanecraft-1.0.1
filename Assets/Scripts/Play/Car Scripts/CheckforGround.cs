using UnityEngine;
using System.Collections;

public class CheckforGround : MonoBehaviour {

    public static int type = 0;
    public static Transform getMid;
    public static int neg = 1;
    private int conti = -1;
    public static int onLine, offLine, onDown, offDown, onTurn, offTurn;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cube"))
        {
            getMid = other.gameObject.transform.parent;
            type = 1;
            onLine = offTurn = offDown = 1; offLine = onDown = onTurn = 0;
        }
        else if (other.name.Contains("CurveDown"))
        {
            getMid = other.gameObject.transform.parent;
            type = 2;
            onDown = offLine = offTurn = 1; onLine = onTurn = offDown = 0;
        }
        else
        {
            onTurn = offLine = offDown = 1; onLine = offTurn = onDown = 0;
            type = 0;
        }
        print(other.name);
        

    }

}
