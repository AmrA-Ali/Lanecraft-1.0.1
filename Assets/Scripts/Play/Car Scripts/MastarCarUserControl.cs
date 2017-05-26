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
       
        public void BrakesDown()
        {
            v = -1;
          //  print("DOWN");
        }
        public void BrakesUp()
        {
            v = 1;
            //print("UP");
        }

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<MastarCarController>();
            v = 1;
        }


        private void FixedUpdate()
        {
          
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
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
