﻿using UnityEngine;

public class FetchOfflineMaps : MonoBehaviour
{
    [SerializeField] private GameObject _mapButton;

    void OnEnable()
    {
        UpdateTheList();
    }

    public void UpdateTheList()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (var m in Map.GetOfflineMaps())
        {
            var gb = Instantiate(_mapButton);
            foreach (var j in gb.GetComponentsInChildren<SelectedMapSetter>())
            {
                j.DisplayMapButton(m);
            }
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}