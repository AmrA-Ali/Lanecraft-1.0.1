using UnityEngine;
public abstract class Checker : MonoBehaviour
{
    void Start()
    {
        var num = int.Parse(gameObject.name.Split('@')[1]);
        if (check() == num) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
    public abstract int check();
}