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
        [SerializeField]
        private float rotationDamping;
        int x = 0;
        void FixedUpdate()
        {
            if (!target)
                return;
            // if (true)//checkground.isGroundforCamera)
            Camera.main.transform.position = Vector3.Lerp(transform.position, 
                WantedPos.position, rotationDamping * Time.deltaTime);
            //Camera.main.transform.position = Vector3.Lerp(transform.position, 
                //new Vector3(0,WantedPos.position.y,WantedPos.position.z), rotationDamping * Time.deltaTime);
            Camera.main.transform.LookAt(MrT);
        }
    }
}