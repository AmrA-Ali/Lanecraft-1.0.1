﻿using UnityEngine;

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
        private float flightdamping = 1, rotationDamping = 30,movableRate=1f;
        [SerializeField]
        private float initFOV = 54, finalFOV = 70;
        [SerializeField]
        private Transform movable;
        void Start()
        {
            transform.eulerAngles = new Vector3(11.6f, 0.2f, 0);
            Camera.main.fieldOfView = initFOV;
            rotationDamping = 20;
        }
        void FixedUpdate()
        {
            Camera.main.transform.position = Vector3.Lerp(transform.position, movable.position,
            /*(CheckforGround.onLine + CheckforGround.onDown + CheckforGround.onTurn) */flightdamping * Time.deltaTime);

            movable.position = Vector3.Lerp(movable.position, 
                CheckforGround.onDown * LongViewPos.position + 
                (CheckforGround.onUp + CheckforGround.onLine +CheckforGround.onTurn) * WantedPos.position,
                    movableRate * Time.deltaTime);
             
            Camera.main.transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation( 
                    CheckforGround.onLine * new Vector3(CheckforGround.getMid.forward.x, transform.forward.y, transform.forward.z)
                    + (CheckforGround.onUp + CheckforGround.onDown + CheckforGround.onTurn) * (MrT.position - transform.position)),
                rotationDamping * Time.deltaTime);

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,
                (CheckforGround.onUp + CheckforGround.onLine + CheckforGround.onDown) * finalFOV
                + CheckforGround.onTurn * initFOV,
                    Time.deltaTime);
        }
    }
}