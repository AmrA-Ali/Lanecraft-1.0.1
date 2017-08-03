using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(test);
    }

    private static void test()
    {
//        Slot.Buy(Slot.LENGTHS[0], (res1,slot) =>
//        {
//            Debug.Log("TEST.BUY: " + res1);
//
//            Map.curr.info.code = "1346411D1152B3A2";
//            Slot.Add(slot, Map.curr, res => { Debug.Log("TEST.ADD " + res); });
//        });
        
        
//        Map.curr.info.code = "1346411D1152B3A2";
//        var s = new Slot
//        {
//            id = "5980e35dff5b8304fd3b5af6",
//            map = Map.curr,
//            length = 1,
//            remaining = 5
//        };
//        Slot.available = new List<Slot> {s};
//        Slot.Remove(Map.curr, res => { Debug.Log("Test.Slot.Remove: " + res); });
        
//        if(!Loading.inst)
//        Loading.StartLoading();
//        else Loading.StopLoading();
        
//        Slot.UpdateFomOnline(() =>
//        {
//            Debug.Log("Test: Done Updating Slot");
//            foreach (var slot in Slot.available)
//            {
//                Debug.Log(slot.GetSaveable());
//            }
//        });
        
        Debug.Log(FB.IsLoggedIn);
    }    
}