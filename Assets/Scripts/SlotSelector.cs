using System;
using LC.Economy;
using UnityEngine;
using UnityEngine.UI;

public class SlotSelector : MonoBehaviour
{
    public GameObject SlotButton;
    public GameObject SlotLengthButton;
    private static Action<bool, int, Slot> _callBack;
    private static GameObject _selectorWindow;

    private void Start()
    {
        GameObject gb;
        //add the lengths to buy from
        foreach (var length in EconomyManager.AvailableSlotLengths())
        {
            gb = Instantiate(SlotLengthButton);
            gb.GetComponentInChildren<BuyedSlotSetter>().SetInfo(length, BuySlotCallBack);
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }

        //add the slots to select from
        foreach (var slot in Slot.GetEmpty())
        {
            gb = Instantiate(SlotButton);
            gb.GetComponentInChildren<SelectedSlotSetter>().SetInfo(slot, SelectSlotCallBack);
            gb.transform.SetParent(transform);
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public static void StartSelectingFrom( Action<bool, int, Slot> cb)
    {
        _callBack = cb;
        var slotSelectorWindow = Resources.Load<GameObject>("Prefabs/UI/SlotSelectorWindow");
        var canvas = GameObject.Find("Canvas");
        _selectorWindow = Instantiate(slotSelectorWindow, canvas.transform);
        _selectorWindow.transform.localScale = new Vector3(1, 1, 1);
        _selectorWindow.GetComponent<Button>().onClick.AddListener(EndSelecting);
    }

    public static void EndSelecting()
    {
        if (_callBack != null)
        {
            Debug.Log("SlotSelector: Cancelled");
            _callBack(true, 0, null);
        }
        Destroy(_selectorWindow);
    }

    public static void SelectSlotCallBack(Slot s)
    {
        EndSelecting();
        _callBack(false, 0, s);
        _callBack = null;
    }

    public static void BuySlotCallBack(int length)
    {
        EndSelecting();
        _callBack(true, length, null);
        _callBack = null;
    }
}