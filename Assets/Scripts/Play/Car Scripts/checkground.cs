    using UnityEngine;
using System.Collections;

public class checkground : MonoBehaviour {

    public static bool isGroundforCamera = false;
    public static bool isGroundforJump = false;
    [SerializeField]
    private float CameraDistGround;
    [SerializeField]
    private float JumpDistGround;
    [SerializeField]
    private Transform Puller;
    // Update is called once per frame
    void Update () {
         //print(isGroundforJump);
        if (IsGrounded())
            isGroundforCamera = true;
        
        else
            isGroundforCamera = false;

        if (IsReallyGrounded())
            isGroundforJump = true;

        else
            isGroundforJump = false;
   

    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position,(Puller.position - transform.position).normalized, CameraDistGround+ 0.1f);
    }
    private bool IsReallyGrounded()
    {
        return Physics.Raycast(transform.position, (Puller.position - transform.position).normalized, JumpDistGround + 0.1f);
    }
    private bool IsGroundedForPull()
    {
        return Physics.Raycast(transform.position, (Puller.position - transform.position).normalized, JumpDistGround + 0.1f);
    }

}
