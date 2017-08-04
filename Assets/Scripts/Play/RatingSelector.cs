using LC.Online;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RatingSelector : MonoBehaviour
{
    void Start()
    {
        Dropdown dd = GetComponent<Dropdown>();
        dd.onValueChanged.AddListener(delegate { myDropdownValueChangedHandler(dd); });
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        Debug.Log("Rating= " + target.value);
        Online.RateMap(Map.Curr.Code, target.value + 1);
        SceneManager.LoadScene("Main");
    }
}