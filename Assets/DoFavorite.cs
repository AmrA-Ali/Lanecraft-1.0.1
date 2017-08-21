using System.Collections;
using System.Collections.Generic;
using LC.Online;
using UnityEngine;
using UnityEngine.UI;

public class DoFavorite : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    void TheAction()
    {
        Online.FavoriteMap(Map.Curr.Code, () => { });
    }
}