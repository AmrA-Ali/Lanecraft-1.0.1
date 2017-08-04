using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class Boost : MonoBehaviour
{
    [SerializeField]
    private MastarCarController m_Car;
    [SerializeField]
    private GameObject boosteffects;
    private bool boostOn;
    public System.Diagnostics.Stopwatch timer, coolDown;
    [SerializeField]
    private float factor = 2, Duration = 3, CoolDown = 8;
    // Update is called once per frame
    private Text boostmsg;
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        timer.Reset();
        boostmsg = transform.GetChild(0).GetComponent<Text>();
        boostmsg.text = "";
        coolDown = new System.Diagnostics.Stopwatch();
        coolDown.Reset();
    }
    void Update()
    {

        if (coolDown.IsRunning || timer.IsRunning)
        { 
            if (timer.Elapsed.Seconds > Duration)
            {
                timer.Stop();
                timer.Reset();
                SpeedDown();
                boostOn = false;
                boosteffects.SetActive(false);
            }
            boostmsg.text = (CoolDown - coolDown.Elapsed.Seconds).ToString();
            
            if (coolDown.Elapsed.Seconds >= CoolDown)
            {
                coolDown.Stop();
                coolDown.Reset();
                gameObject.GetComponent<Button>().interactable = true;
                boostmsg.text = "";
            }
        }

    }
    public void BoostOn()
    {
        if (!boostOn)
        {
            timer.Reset();
            timer.Start();
            coolDown.Reset();
            coolDown.Start();
            SpeedUp();
            gameObject.GetComponent<Button>().interactable = false;
            boostOn = true;
            boosteffects.SetActive(true);
        }
    }
    void SpeedUp()
    {
        m_Car.m_FullTorqueOverAllWheels *= factor;
        m_Car.m_Topspeed *= factor;
        boostmsg.text = "";
    }

    void SpeedDown()
    {
        m_Car.m_FullTorqueOverAllWheels /= factor;
        m_Car.m_Topspeed /= factor;
    }

}
