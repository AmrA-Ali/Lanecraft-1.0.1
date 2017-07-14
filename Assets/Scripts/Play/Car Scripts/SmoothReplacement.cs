using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class SmoothReplacement : MonoBehaviour
    {

        // The target we are following
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private Transform MrT;
        [SerializeField]
        private Transform WantedPos;
        // The distance in the x-z plane to the target
        // [SerializeField]
        // private float flightdamping = 1;
        [SerializeField]
        private float rotationDamping;
        [SerializeField]
        private float lookatdamp;
        // int x = 0;
        void FixedUpdate()
        {
            if (!target)
                return;
            // if (true)//checkground.isGroundforCamera)
               Camera.main.transform.position = Vector3.Lerp(transform.position, WantedPos.position, rotationDamping * Time.deltaTime);
            //  Camera.main.transform.position = Vector3.Lerp(transform.position, new Vector3(0,WantedPos.position.y,WantedPos.position.z), rotationDamping * Time.deltaTime);
            Camera.main.transform.LookAt(MrT);
        }
    }
}