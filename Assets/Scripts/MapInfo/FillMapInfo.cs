﻿using UnityEngine;
using UnityEngine.UI;

public class FillMapInfo : MonoBehaviour
{
    public Text Name;
    public Text Code;
    public Text Creator;
    public Text DateCreated;
    public Text DateUpdated;
    public Text BrickCount;
    public Text Difficulty;
    public Text HighScore;
    public Text Statistics;
    public Text IsOnline;
    public Text IsOffline;
    public Text IsMine;
    public Text IsShared;

    public GameObject DeleteButton;
    public GameObject AddButton;
    public GameObject RemoveButton;
    public GameObject PlayButton;


    // Use this for initialization
    void Start()
    {
        var t = Map.Curr.Info;
        Name.text = t.Name;
        Code.text = Map.Curr.Code;
        Creator.text = t.Creator;
        DateCreated.text = t.DateCreated.ToString();
        DateUpdated.text = t.DateUpdated.ToString();
        BrickCount.text = t.BrickCount.ToString();
        Difficulty.text = t.Difficulty.ToString();
        HighScore.text = t.HighestScore.ToString();
        Statistics.text = "Turn Rights: " + t.Statistics.TurnRights + "\n" +
                          "Turn Lefts: " + t.Statistics.TurnLefts + "\n" +
                          "Curve Ups: " + t.Statistics.CurveUps + "\n" +
                          "Curve Downs: " + t.Statistics.CurveDowns + "\n" +
                          "Lines: " + t.Statistics.Lines + "\n" +
                          "Obstacles: " + t.Statistics.ObstacleCount;

        IsOnline.text = Map.Curr.IsOnline.ToString();
        IsOffline.text = Map.Curr.IsOffline.ToString();
        IsMine.text = Map.Curr.IsMine.ToString();
        IsShared.text = Map.Curr.IsShared.ToString();

        AddButton.SetActive(false);
        RemoveButton.SetActive(false);
        PlayButton.SetActive(true); //play is always available
        DeleteButton.SetActive((Map.Curr.IsOffline && !Map.Curr.IsMine) || (Map.Curr.IsMine && !Map.Curr.IsShared));

        //Activate the publish button if it's offline map and belong to the user and not published already
        if (Map.Curr.IsOffline)
        {
            //The map is offline now check that it belongs to the user
            if (Map.Curr.IsMine)
            {
                //the map belongs to the player, now check that if it's shared already
                if (!Map.Curr.IsShared)
                {
                    //the map is not online, show the add button to attache the map to a slot
                    AddButton.SetActive(true);
                }
                else
                {
                    //the map is already online, show the remove button to dettache the map from the slot
                    RemoveButton.SetActive(true);
                }
            }
        }
    }
}