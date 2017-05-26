using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;
public class Boost : MonoBehaviour {
    private GameObject car;
    private MastarCarController MC;
	void Start ()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        MC = car.GetComponent<MastarCarController>();
        #region onclicks
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry PointerDown = new EventTrigger.Entry(), PointerUp = new EventTrigger.Entry();
        PointerDown.eventID = EventTriggerType.PointerDown;
        PointerUp.eventID = EventTriggerType.PointerUp;
        PointerUp.callback.AddListener((data) => { BoostOff(); });
        PointerDown.callback.AddListener((data) => { BoostOn(); });
        trigger.triggers.Add(PointerUp);
        trigger.triggers.Add(PointerDown);
        #endregion
    }
    public void BoostOn()
    {
        transform.GetChild(0).GetComponent<Text>().text = "Chill";
        MC.m_FullTorqueOverAllWheels *= 2;
        MC.m_Topspeed *= 2;
    }
    public void BoostOff()
    {
        transform.GetChild(0).GetComponent<Text>().text = "BOOST";
        MC.m_FullTorqueOverAllWheels /= 2;
        MC.m_Topspeed /= 2;
    }

}
