using UnityEngine;
using UnityEngine.UI;

public class LoginWithFB : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Player.LoginFb);
    }
}