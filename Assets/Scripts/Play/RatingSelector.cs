using LC.Online;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RatingSelector : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    void TheAction()
    {
        Dropdown dd = transform.parent.gameObject.GetComponentInChildren<Dropdown>();
        if (dd == null)
        {
            return;
        }
        Debug.Log("Rating= " + dd.value);
        Online.RateMap(Map.Curr.Code, dd.value + 1, () =>
        {
            Debug.Log("Rating Done");
        });
    }
}