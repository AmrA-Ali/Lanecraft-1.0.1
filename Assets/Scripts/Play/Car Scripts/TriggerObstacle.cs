using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class TriggerObstacle : MonoBehaviour {

    [SerializeField]
    private MastarCarController m_Car;
    public System.Diagnostics.Stopwatch timer, coolDown;
    [SerializeField]
    private float factor = 2, Duration = 3, defaultTorque = 300;

    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        timer.Reset();
    }
    void Update()
    {
        if (timer.IsRunning)
        {
            if (timer.Elapsed.Seconds > Duration)
            {
                timer.Stop();
                timer.Reset();
                SpeedUp();
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "obstacle")
        {
            Hit();
            other.gameObject.SetActive(false);
        }
    }
    public void Hit()
    {
        timer.Reset();
        timer.Start();
        SpeedDown();
    }
    void SpeedDown()
    {
        m_Car.m_FullTorqueOverAllWheels *= factor;
    }

    void SpeedUp()
    {
        m_Car.m_FullTorqueOverAllWheels = defaultTorque;
    }
}
