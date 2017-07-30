using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingSelector : MonoBehaviour
{
    void Start()
    {
        Dropdown dd = GetComponent<Dropdown>();
        dd.onValueChanged.AddListener(delegate { myDropdownValueChangedHandler(dd); });
    }

    private static void myDropdownValueChangedHandler(Dropdown target)
    {
        Debug.Log("Rating= " + target.value);
        Online.RateMap(Map.curr.info.code, target.value + 1);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}