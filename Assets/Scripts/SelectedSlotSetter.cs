using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSlotSetter : MonoBehaviour
{
    private Action<Slot> callBack;
    private Slot slot;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TheAction);
    }

    private void TheAction()
    {
        callBack(slot);
    }

    public void SetInfo(Slot s, Action<Slot> cb)
    {
        slot = s;
        callBack = cb;
        //Display the slot info
        transform.GetComponentInChildren<Text>().text =
            "Remaining: " + slot.Remaining + ", out of: " + slot.Length * 24;
    }
}