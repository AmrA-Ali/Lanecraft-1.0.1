using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (MastarCarController))]
    public class MastarCarUserControl : MonoBehaviour
    {
        private MastarCarController m_Car; // the car controller we want to use
        public float v = 1;
        [SerializeField]
        private float touchAngVel = 5;
        [SerializeField]
        private float touchReturn = 10;
        public int checkdirection,noReturn;

        private float h;
        public GameObject PlayModeButtons;
        public void BrakesDown()
        {   v = -1; }
        public void BrakesUp()
        { v = 1; }

        public void TurnManager(int dir)
        {
            checkdirection = dir;
            if (dir != 0) noReturn = 0;
            else noReturn = 1;
        }
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<MastarCarController>();
            v = 1;
        }

        private void FixedUpdate()
        {

                h = Mathf.Lerp(h, checkdirection * 0.5f, ControlSwitcher.stopAxis* 
                    Time.deltaTime * (touchAngVel*Mathf.Abs(checkdirection) + touchReturn*noReturn));
                h = (CrossPlatformInputManager.GetAxis("Horizontal")*ControlSwitcher.AcceloMode + h*ControlSwitcher.stopAxis);
            //   float v = CrossPlatformInputManager.GetAxis("Vertical");

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);

#else
            //if (Mathf.Abs(h) < 0.1f)
            //   {
            //      h = 0;
            //   }
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
