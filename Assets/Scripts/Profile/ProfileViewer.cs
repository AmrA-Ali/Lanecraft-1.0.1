using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewer : MonoBehaviour
{
	public Text Name;
	public Text UID;
	public Text FBID;
	public Image PIC;

	// Use this for initialization
	void Start ()
	{
		Name.text = Auth.NAME;
		UID.text = Auth.UID;
		FBID.text = Auth.FBID;
		PIC.material.mainTexture = Auth.FBPIC;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
