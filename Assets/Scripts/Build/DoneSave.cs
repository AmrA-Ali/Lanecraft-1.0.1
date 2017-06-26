﻿using UnityEngine;
using UnityEngine.UI;
public class DoneSave : MonoBehaviour {
    [SerializeField]
	private InputField mapName;
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { Confirm(); });
	}
    void Confirm()  {
        Map.curr.info.name = mapName.text;
        Map.curr.Save();
        Map.curr = new global::Map();
    }
}
