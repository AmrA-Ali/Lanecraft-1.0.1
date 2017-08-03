
using UnityEngine;
using UnityEngine.UI;

public class DoStartSharing : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    private static void TheAction()
    {
        //get the empty slots we have
        var slots = Slot.GetEmpty();
        //ask the user if wants to use them or buy new 
        SlotSelector.StartSelectingFrom(slots, (buy, length, slot) =>
        {
            if (buy)
            {
                if (length == 0) return; //cancelled selecting the length
                //buy new slot
                Slot.Buy(length, (res, newSlot) =>
                {
                    if (!res) return; //problem buying the slot
                    UploadMapAndAddit(newSlot);
                });
                //then use it
            }
            else
            {
                //if yes use it
                if (slot == null) return; //cancelled selecting the slot
                UploadMapAndAddit(slot);
            }
        });
    }

    private static void UploadMapAndAddit(Slot slot)
    {
        Loading.StartLoading();
        //check the map is online, if not send it
        Map.Curr.Upload(() =>
        {
            //add the map to the slot
            Slot.Add(slot, Map.Curr, res =>
            {
                //todo refresh the affected things, like maps
                Debug.Log("StartSharing :" + res);
                Loading.StopLoading();
            });
        });
    }
}