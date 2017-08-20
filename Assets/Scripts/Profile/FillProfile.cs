using UnityEngine;
using UnityEngine.UI;

public class FillProfile : MonoBehaviour
{
	public Text Name;
	public Text Uid;
	public Text Fbid;
	public Image Pic;

	// Use this for initialization
	void Start ()
	{
		Name.text = Player.Data.Name;
		Uid.text = Player.Data.Id;
		Fbid.text = Player.Data.Fbid;
		Pic.material.mainTexture = Player.Data.Fbpic;
	}
}
