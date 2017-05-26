using UnityEngine;
using System.Collections;

public class CheckforGround : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
     //   print("ONGROUND");
    }
    void OnTriggerExit(Collider other)
    {
       // print("OFFGROUND    ");
    }
}
