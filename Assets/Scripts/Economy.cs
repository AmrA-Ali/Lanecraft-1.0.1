using System;
using UnityEngine;

namespace LC.Economy
{
    public class EconomyManager
    {
        public static int[] AvailableSlotLengths()
        {
            return Array.FindAll(Slot.Lengths, CanBuySlot);
        }

        public static bool CanFavoriteMap()
        {
            Debug.Log("Economy: " + "CanFavoriteMap");
            return true;
        }

        public static bool CanSaveMap()
        {
            Debug.Log("Economy: " + "CanSaveMap");
            return true;
        }

        public static bool CanBuySlot(int length)
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

        public static void GetReady(Action callBack)
        {
            callBack();
        }
    }
}