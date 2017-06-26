using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FillMapInfo : MonoBehaviour
{
	public Text name;
	public Text Code;
	public Text Creator;
	public Text DateCreated;
	public Text DateUpdated;
	public Text BrickCount;
	public Text Difficulty;
	public Text HighScore;
	public Text Statistics;

	public GameObject DeleteButton;
	public GameObject UploadButton;
	public GameObject DownloadButton;

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

		DeleteButton.SetActive (Map.curr.isOffline);
		UploadButton.SetActive (Map.curr.isMine);
		DownloadButton.SetActive (!Map.curr.isMine);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
