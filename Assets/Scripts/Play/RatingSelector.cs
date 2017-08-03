using UnityEngine;
using UnityEngine.UI;
using LC.Online;

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
        Online.RateMap(Map.Curr.Info.Code, target.value + 1);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}