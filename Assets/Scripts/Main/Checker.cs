using UnityEngine;
public abstract class Checker : MonoBehaviour
{
    void Start()
    {
        var num = int.Parse(gameObject.name.Split('@')[1]);
        if (Check() == num) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    public abstract int Check();
}