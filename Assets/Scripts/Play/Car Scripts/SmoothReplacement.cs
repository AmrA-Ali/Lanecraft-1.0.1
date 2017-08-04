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
        private Transform WantedPos, LongViewPos;
        // The distance in the x-z plane to the target
        [SerializeField]
        private float flightdamping = 1;
        [SerializeField]
        private float rotationDamping = 30;
        [SerializeField]
        private float lookatdamp;
        int x = 0;
        Rigidbody gbrob;
        [SerializeField]
        private float lookupRate = 16;
        [SerializeField]
        private float lookupAngle = 11;
        [SerializeField]
        private float initFOV = 54, finalFOV = 70;
        [SerializeField]
        private Transform movable;
        void Start()
        {
            transform.eulerAngles = new Vector3(11.6f, 0.2f, 0);
            Camera.main.fieldOfView = initFOV;
            gbrob = MrT.parent.gameObject.GetComponent<Rigidbody>();
            
        }
        void FixedUpdate()
        {
            if (CheckforGround.type == 1)
            {
                Camera.main.transform.position = Vector3.Lerp(transform.position, WantedPos.position,
            flightdamping * Time.deltaTime);

                Camera.main.transform.rotation = Quaternion.Lerp(transform.rotation,
                  //Quaternion.LookRotation(CheckforGround.getMid.forward, new Vector3(0,1,0)),//CheckforGround.neg*CheckforGround.getMid.up),
                  Quaternion.LookRotation(
                      new Vector3(CheckforGround.getMid.forward.x, MrT.forward.y, MrT.forward.z),
                      new Vector3(0, 1, 0)),//CheckforGround.neg*CheckforGround.getMid.up),
                  rotationDamping * Time.deltaTime);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, finalFOV, Time.deltaTime);
            }
            else if (CheckforGround.type == 2)
            {
                Camera.main.transform.position = Vector3.Lerp(transform.position, LongViewPos.position,
            flightdamping * Time.deltaTime);

                Camera.main.transform.LookAt(MrT);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, finalFOV, Time.deltaTime);
            }

            else {
                Camera.main.transform.position = Vector3.Lerp(transform.position, WantedPos.position,
                flightdamping * Time.deltaTime);
                Camera.main.transform.rotation = Quaternion.Lerp(transform.rotation,
                 Quaternion.LookRotation((MrT.position - transform.position)),
                rotationDamping* 5 * Time.deltaTime);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initFOV, Time.deltaTime);
                //Camera.main.transform.LookAt(MrT);
            }


        }
    }
}