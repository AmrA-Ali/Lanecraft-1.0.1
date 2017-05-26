using UnityEngine;
using System.Collections;
using System;

public class CheckThereAreMaps : Checker {
    public override int check()
    {
        return CountMaps.ThereAreMaps ? 1 : 0;
    }
}
