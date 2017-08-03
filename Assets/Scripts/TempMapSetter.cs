using UnityEngine;

public class TempMapSetter : MonoBehaviour
{
    void Start()
    {
        gameObject.SetMap(Map.LoadTemp());
    }

}