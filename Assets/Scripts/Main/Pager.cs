using UnityEngine;
using UnityEngine.UI;
public class Pager : MonoBehaviour {
    void Start () { GetComponent<Button>().onClick.AddListener(delegate { gameObject.SwitchPage("page"); });}
}
