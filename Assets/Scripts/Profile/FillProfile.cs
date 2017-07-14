using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillProfile : MonoBehaviour
{
	public Text Name;
	public Text UID;
	public Text FBID;
	public Image PIC;

	// Use this for initialization
	void Start ()
	{
		Name.text = Player.DATA.name;
		UID.text = Player.DATA.id;
		FBID.text = Player.DATA.fbid;
		PIC.material.mainTexture = Player.DATA.fbpic;
	}
}
