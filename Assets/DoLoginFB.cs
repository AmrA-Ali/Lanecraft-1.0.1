using UnityEngine;
using UnityEngine.UI;

public class DoLoginFb : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Player.LoginFb);
    }
}