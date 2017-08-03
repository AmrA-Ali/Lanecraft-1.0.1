using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyedSlotSetter : MonoBehaviour {

	private Action<int> callBack;
	private int length;

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(TheAction);
	}

	private void TheAction()
	{
		callBack(length);
	}

	public void SetInfo(int l, Action<int> cb)
	{
		callBack = cb;
		length = l;
		//Display the slot info
		transform.GetComponentInChildren<Text>().text =
			"Buy: " + length * 24 + " Hours";
	}
}
