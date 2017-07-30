using System;
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
        public int checkdirection;

        private float h;
        public GameObject PlayModeButtons;
        public void BrakesDown()
        {   v = -1; }
        public void BrakesUp()
        { v = 1; }

        public void TurnManager(int dir)
        { checkdirection = dir; }
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<MastarCarController>();
            v = 1;
        }

        private void FixedUpdate()
        {

            if (!ControlSwitcher.AcceloMode)
            {
                if (checkdirection == 1)
                    h = Mathf.Lerp(h, 0.5f, Time.deltaTime * touchAngVel);
                else if (checkdirection == -1)
                    h = Mathf.Lerp(h, -0.5f, Time.deltaTime * touchAngVel);
                else h = Mathf.Lerp(h, 0, Time.deltaTime * touchReturn);
            }
            else
                // pass the input to the car!
                h = CrossPlatformInputManager.GetAxis("Horizontal");
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
