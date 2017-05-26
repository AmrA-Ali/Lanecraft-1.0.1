using UnityEngine;
using System.Collections;

public class SavingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {//hello
        Map mm = new Map();
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("TurnRight",true);
        mm.AddBrick("TurnRight",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("Line",true);
        mm.AddBrick("TurnRight",true);
        mm.AddBrick("TurnRight",true);
        mm.AddBrick("CurveUp", true);
        mm.AddBrick("CurveUp", true);
        mm.info.name = "The Third Track";
        mm.Save();
    }
	
}
