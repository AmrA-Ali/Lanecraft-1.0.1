using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotSelector : MonoBehaviour
{
    private static List<Slot> slotsList;
    public GameObject SlotButton;
    public GameObject SlotLengthButton;
    private static Action<bool, int, Slot> callBack;
    private static GameObject selectorWindow;
    private void Start()
    {
        GameObject gb;
        //add the lengths to buy from
        foreach (var length in Slot.Lengths)
        {
            gb = Instantiate(SlotLengthButton);
            gb.GetComponentInChildren<BuyedSlotSetter>().SetInfo(length, BuySlotCallBack);
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
        //add the slots to select from
        foreach (var slot in slotsList)
        {
            gb = Instantiate(SlotButton);
            gb.GetComponentInChildren<SelectedSlotSetter>().SetInfo(slot, SelectSlotCallBack);
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }

    public static void StartSelectingFrom(List<Slot> slots, Action<bool, int, Slot> cb)
    {
        slotsList = slots;
        callBack = cb;
        var SlotSelectorWindow = Resources.Load<GameObject>("Prefabs/UI/SlotSelectorWindow");
        var canvas = GameObject.Find("Canvas");
        selectorWindow = Instantiate(SlotSelectorWindow, canvas.transform);
        selectorWindow.transform.localScale = new Vector3(1, 1, 1);
    }

    public static void EndSelecting()
    {
        if (callBack != null)
            callBack(true, 0, null);
        Destroy(selectorWindow);
    }

    public static void SelectSlotCallBack(Slot s)
    {
        EndSelecting();
        callBack(false, 0, s);
        callBack = null;
        
    }

    public static void BuySlotCallBack(int length)
    {
        EndSelecting();
        callBack(true, length, null);
        callBack = null;
    }
}