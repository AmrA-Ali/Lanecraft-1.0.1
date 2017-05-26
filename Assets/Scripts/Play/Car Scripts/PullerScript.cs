using UnityEngine;
using System.Collections;

public class PullerScript : MonoBehaviour {

    // Use this for initialization
    private Rigidbody rb;
    public Transform puller;
    [SerializeField]
    private float PerpGravity;
	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	// Update is called once per frame
	void Update () {
        if (checkground.isGroundforCamera) 
        rb.AddForce((puller.position - transform.position) * PerpGravity);
	}
}
