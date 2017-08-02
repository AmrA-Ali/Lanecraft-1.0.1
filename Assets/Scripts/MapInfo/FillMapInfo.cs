using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class FillMapInfo : MonoBehaviour
{
	public new Text name;
	public Text Code;
	public Text Creator;
	public Text DateCreated;
	public Text DateUpdated;
	public Text BrickCount;
	public Text Difficulty;
	public Text HighScore;
	public Text Statistics;

	public GameObject DeleteButton;
	public GameObject AddButton;
	public GameObject RemoveButton;
	public GameObject PlayButton;
	
	

	// Use this for initialization
	void Start ()
	{
		var t = Map.curr.info;
		name.text = t.name;
		Code.text = t.code;
		Creator.text = t.creator;
		DateCreated.text = t.dateCreated.ToString ();
		DateUpdated.text = t.dateUpdated.ToString ();
		BrickCount.text = t.brickCount.ToString ();
		Difficulty.text = t.difficulty.ToString ();
		HighScore.text = t.highestScore.ToString ();
		Statistics.text = "Turn Rights: " + t.statistics.turnRights + "\n" +
		"Turn Lefts: " + t.statistics.turnLefts + "\n" +
		"Curve Ups: " + t.statistics.curveUps + "\n" +
		"Curve Downs: " + t.statistics.curveDowns + "\n" +
		"Lines: " + t.statistics.lines + "\n" +
		"Obstacles: " + t.statistics.obstacleCount;

		//Activate the publish button if it's offline map and belong to the user and not published already
		if (Map.curr.isOffline)
		{
			//The map is offline now check that it belongs to the user
			if (Map.curr.info.creator == Player.DATA.Creator())
			{
				//the map belongs to the player, now check that if it's shared already
				if (Map.curr.slot == null)
				{
					//the map is not online, show the add button to attache the map to a slot
					AddButton.SetActive(true);
				}
				else
				{
					//the map is already online, show the remove button to dettache the map from the slot
					RemoveButton.SetActive(false);
				}
				
			}
		}
		
		DeleteButton.SetActive (Map.curr.isOffline);
		
		PlayButton.SetActive(true);//play is always available
	}
	
}
