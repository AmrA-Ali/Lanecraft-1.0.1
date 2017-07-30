using UnityEngine;
using UnityEngine.UI;

public class TempMapSetter : MonoBehaviour
{
    void Start()
    {
        gameObject.setMap(Map.LoadTemp());
    }

}