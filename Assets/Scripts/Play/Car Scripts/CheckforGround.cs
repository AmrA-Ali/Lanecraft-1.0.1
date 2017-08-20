using UnityEngine;
using System.Collections;

public class CheckforGround : MonoBehaviour {

    public static int type = 0;
    public static Transform getMid;
    public static int neg = 1;
    public static int onLine, onDown,  onTurn,onUp;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cube"))
        {
            getMid = other.gameObject.transform.parent;
            onLine = 1; onDown = onTurn = onUp = 0;
        }
        else if (other.name.Contains("CurveDown"))
        {
            getMid = other.gameObject.transform.parent;
            onDown = 1; onLine = onTurn = onUp = 0;
        }
        else if (other.name.Contains("CurveUp"))
        {
            getMid = other.gameObject.transform.parent;
            onUp = 1;  onTurn = onLine = onDown = 0;
        }
        else 
        {
            onTurn = 1; onUp = onLine = onDown = 0;
        }
        print(other.name);
        

    }

}
