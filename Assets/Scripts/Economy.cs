using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC.Economy
{
    public class EconomyManager
    {
        public static bool CanSaveMap()
        {
            Debug.Log("Economy: " + "CanSaveMap");
            return true;
        }

        public static bool CanBuySlot()
        {
            Debug.Log("Economy: " + "CanBuySlot");
            return true;
        }

        public static bool CanQuickPlay()
        {
            Debug.Log("Economy: " + "CanQuickPlay");
            return true;
        }

        public static bool CanSelectPlay()
        {
            Debug.Log("Economy: " + "CanSelectPlay");
            return true;
        }

        public static bool CanPublishMap()
        {
            Debug.Log("Economy: " + "CanPublishMap");
            return true;
        }
    }
}